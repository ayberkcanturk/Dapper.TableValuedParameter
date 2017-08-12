using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Dapper.TableValuedParameter.Extensions;

namespace Dapper.TableValuedParameter.Console
{
    public class Author
    {
    }

    public class TvpDto
    {
    }

    public class TvpExample
    {
        public IEnumerable<Author> Query()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString))
            {
                IEnumerable<TvpDto> dto = new List<TvpDto>();

                return db.Query<Author>("dbo.GetAuthor", new Tvp("@Test", "dbo.UserDefinedType", dto), commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
