using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dapper.TableValuedParameter.ConsoleCore
{
    public class Author
    {
    }

    public class TvpDto
    {
    }

    public class TvpExample
    {
        private readonly string _connectionString;

        public TvpExample(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Author> Query()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                IEnumerable<TvpDto> dto = new List<TvpDto>();

                return db.Query<Author>("dbo.GetAuthor", new Tvp("@Test", "dbo.UserDefinedType", dto), commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
