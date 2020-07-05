using System;
using System.Linq;
using SearchFight.Logic;
using SearchFight.Services.Interfaces;

namespace SearchFight.Clients
{
    public class SearchFightClients
    {        
        public static SearchManager CreateSearchClients()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                ?.Where(assembly => assembly.FullName.StartsWith("SearchFight"));

            var searchClients = loadedAssemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetInterface(typeof(ISearchClient).ToString()) != null)
                .Select(type => Activator.CreateInstance(type) as ISearchClient);

            return new SearchManager(searchClients);
        }
    }
}
