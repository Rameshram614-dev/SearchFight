using System;
using System.Net.Http;
using System.Threading.Tasks;
using SearchFight.Helper.Config;
using SearchFight.Helper.Exceptions;
using SearchFight.Helper.Extensions;
using SearchFight.Services.Interfaces;
using SearchFight.Services.Models.Bing;

namespace SearchFight.Services.SearchEngines
{
    public class BingSearchClient : ISearchClient
    {
        public string ClientName => "Bing Search";
        private static readonly HttpClient _httpClient;
       private string _bingUrl = ConfigManager.BingUri;
        static BingSearchClient()
        {           
            _httpClient = new HttpClient
            {                
                DefaultRequestHeaders = { { "Ocp-Apim-Subscription-Key", ConfigManager.BingKey } }
            };
        }

        public async Task<long> GetResultsCountAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));

            try
            {
                using (var response = await _httpClient.GetAsync(_bingUrl.Replace("{3}",query)))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new SearchFightException(
                            "An error occuured. Please try again later...");

                    var result = await response.Content.ReadAsStringAsync();
                    var bingResponse = result.DeserializeJson<BingResponse>();
                    return long.Parse(bingResponse.WebPages.TotalEstimatedMatches);
                }
            }
            catch (Exception ex)
            {
                throw new SearchFightException(ex.Message);
            }
        }
    }
}
