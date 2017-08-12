using System.Collections.Generic;

namespace Dapper.TableValuedParameter.Console
{
    class Program
    {
        public Program()
        {
        }
        static void Main(string[] args)
        {
            TvpExample e = new TvpExample();
            IEnumerable<Author> authors = e.Query();
        }
    }
}