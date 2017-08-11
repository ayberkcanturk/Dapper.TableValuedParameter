using System.Data;

using Dapper.TableValuedParameter;

namespace Dapper.TableValueParameter.Extensions
{
    /// <summary>
    ///     DbConnectionExtensions
    /// </summary>
    public static class DbConnectionExtensions
    {
        public static void Query<TAny>(this IDbConnection connection, string sql, Tvp tvp, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) where TAny : class
        {
            //Tvp as object to prevent recursive loop
            connection.Query<TAny>(sql, tvp as object, transaction, buffered, commandTimeout, commandType);
        }
    }
}
