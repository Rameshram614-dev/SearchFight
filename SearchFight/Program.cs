using SearchFight.Common.Exceptions;
using SearchFight.Clients;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SearchFight
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Please enter key word to search....");
                    args = Console.ReadLine()?.Split(' ');
                }

                Console.WriteLine("Loading...");

                var searchManager = SearchFightClients.CreateSearchClients();
                var result = await searchManager.GetSearchReport(args?.ToList());

                Console.Clear();
                Console.WriteLine(result);
            
            }
            catch (SearchFightException ex)
            {
                Console.WriteLine(ex.Message);
            }           
            
        }
    }
}
