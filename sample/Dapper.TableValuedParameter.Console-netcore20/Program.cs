using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace Dapper.TableValuedParameter.ConsoleCore
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            TvpExample e = new TvpExample(Configuration.GetConnectionString("AdventureWorks"));
            IEnumerable<Author> authors = e.Query();
        }
    }
}