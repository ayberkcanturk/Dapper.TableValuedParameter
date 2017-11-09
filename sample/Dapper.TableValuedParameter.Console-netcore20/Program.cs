using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace Dapper.TableValuedParameter.ConsoleCore
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static void Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var e = new TvpExample(Configuration.GetConnectionString("AdventureWorks"));
            IEnumerable<Author> authors = e.Query();
        }
    }
}
