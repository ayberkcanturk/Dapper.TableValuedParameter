using System.Collections.Generic;

namespace Dapper.TableValuedParameter.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var e = new TvpExample();
            IEnumerable<Author> authors = e.Query();
        }
    }
}
