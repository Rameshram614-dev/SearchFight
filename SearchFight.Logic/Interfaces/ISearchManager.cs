using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SearchFight.Logic.Models;

namespace SearchFight.Logic
{
    public interface ISearchManager
    {
        Task<string> GetSearchReport(List<string> querys);
        Task<List<SearchResult>> GetSearchResults(IEnumerable<string> querys);
        IEnumerable<Winner> GetWinners(List<SearchResult> searchResults);
        string GetTotalWinner(List<SearchResult> searchResults);
        IEnumerable<IGrouping<string, SearchResult>> GetMainResults(List<SearchResult> searchResults);
    }
}
