using System.Collections.Generic;
using System.Data;

namespace Dapper.TableValuedParameter.Extensions
{
    /// <summary>
    ///     DbConnectionExtensions
    /// </summary>
    public static class DbConnectionExtensions
    {
        public static IEnumerable<TAny> Query<TAny>(this IDbConnection connection, string sql, Tvp tvp, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where TAny : class
        {
            //Tvp as object to prevent recursive loop
            return connection.Query<TAny>(sql, tvp as object, transaction, buffered, commandTimeout, commandType);
        }
    }
}
