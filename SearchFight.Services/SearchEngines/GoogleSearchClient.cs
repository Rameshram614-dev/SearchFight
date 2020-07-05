using System;
using System.Net.Http;
using System.Threading.Tasks;
using SearchFight.Helper.Config;
using SearchFight.Services.Interfaces;
using SearchFight.Services.Models.Google;
using SearchFight.Helper.Exceptions;
using SearchFight.Helper.Extensions;

namespace SearchFight.Services.SearchEngines
{
    public class GoogleSearchClient : ISearchClient
    {
        public string ClientName => "Google Search";
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _googleUrl;

        public GoogleSearchClient()
        {
            _googleUrl = ConfigManager.GoogleUri
                .Replace("{0}", ConfigManager.GoogleKey)
                .Replace("{1}", ConfigManager.GoogleCxKey);
        }

        public async Task<long> GetResultsCountAsync(string query)
        {

            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));

            try
            {
                using (var response = await _httpClient.GetAsync(_googleUrl.Replace("{2}", query)))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new SearchFightException(
                            "An error occured. Please try again later...");

                    var result = await response.Content.ReadAsStringAsync();
                    var googleResponse = result.DeserializeJson<GoogleResponse>();
                    return long.Parse(googleResponse.SearchInformation.TotalResults);
                }
            }
            catch (Exception ex)
            {
                throw new SearchFightException(ex.Message);
            }
        }
    }
}
