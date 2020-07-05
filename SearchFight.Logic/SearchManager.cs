using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchFight.Common.Exceptions;
using SearchFight.Common.Extensions;
using SearchFight.Logic.Models;
using SearchFight.Services.Interfaces;



namespace SearchFight.Logic
{
    public class SearchManager : ISearchManager
    {
        private readonly IEnumerable<ISearchClient> _searchClients;
        private readonly StringBuilder _stringBuilder;

        public SearchManager(IEnumerable<ISearchClient> searchClients)
        {
            _searchClients = searchClients;
            _stringBuilder = new StringBuilder();
        }

        public async Task<string> GetSearchReport(List<string> querys)
        {
            
            try
            {
                var searchResults = await GetSearchResults(querys.Distinct());

                var winnners = GetWinners(searchResults);
                var totalWinner = GetTotalWinner(searchResults);
                var mainResults = GetMainResults(searchResults);

                var clientResultsString = mainResults
                    .Select(resultsGroup =>
                        $"{resultsGroup.Key}: {string.Join(" ", resultsGroup.Select(client => $"{client.SearchEngine}: {client.TotalResults}"))}")
                    .ToList();

                var winnerString = winnners.Select(client => $"{client.SearchEngineName} winner: {client.WinnerQuery}")
                    .ToList();

                var totallWinnerString = $"Total winner: {totalWinner}";

                clientResultsString.ForEach(queryResults => _stringBuilder.AppendLine(queryResults));
                winnerString.ForEach(winners => _stringBuilder.AppendLine(winners));

                _stringBuilder.AppendLine(totallWinnerString);

                return _stringBuilder.ToString();
            }
            catch (Exception e)
            {
                throw new SearchFightException(e.Message);
            }            
        }

        public IEnumerable<Winner> GetWinners(List<SearchResult> searchResults)
        {
         
            var winners = searchResults
                .OrderBy(result => result.SearchEngine)
                .GroupBy(result => result.SearchEngine, result => result,
                    (client, result) => new Winner
                    {
                        SearchEngineName = client,
                        WinnerQuery = result.MaxValue(r => r.TotalResults).Query
                    });

            return winners;
        }

        public string GetTotalWinner(List<SearchResult> searchResults)
        {
           
            var totalWinner = searchResults
                .OrderBy(result => result.SearchEngine)
                .GroupBy(result => result.Query, result => result,
                    (query, result) => new { Query = query, Total = result.Sum(r => r.TotalResults) })
                .MaxValue(r => r.Total).Query;

            return totalWinner;
        }

        public IEnumerable<IGrouping<string, SearchResult>> GetMainResults(List<SearchResult> searchResults)
        {
          
            var results = searchResults
                .OrderBy(result => result.SearchEngine)
                .ToLookup(result => result.Query, result => result);

            return results;
        }

        public async Task<List<SearchResult>> GetSearchResults(IEnumerable<string> querys)
        {
            var results = new List<SearchResult>();

            foreach (var keyword in querys)
            {
                foreach (var searchClient in _searchClients)
                {
                    results.Add(new SearchResult
                    {
                        SearchEngine = searchClient.ClientName,
                        Query = keyword,
                        TotalResults = await searchClient.GetResultsCountAsync(keyword)
                    });
                }
            }

            return results;
        }
    }
}
