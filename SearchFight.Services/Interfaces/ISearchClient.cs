using System.Threading.Tasks;

namespace SearchFight.Services.Interfaces
{
    public interface ISearchClient
    {
        string ClientName { get; }
        Task<long> GetResultsCountAsync(string query);
    }
}
