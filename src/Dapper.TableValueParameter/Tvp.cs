using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Microsoft.SqlServer.Server;

namespace Dapper.TableValuedParameter
{
    public class Tvp : SqlMapper.IDynamicParameters
    {
        private readonly string _parameterName;
        private readonly IEnumerable<SqlDataRecord> _rows;
        private readonly string _typeName;

        public Tvp(string parameterName, string typeName, 
            IEnumerable<object> rows, 
            TypeSqlDbTypeMap typeSqlDbType = null)
        {
            _parameterName = parameterName;
            _typeName = typeName;

            if (typeSqlDbType== null)
            {
                typeSqlDbType = new TypeSqlDbTypeMap();
            }
            var genericTvp = new GenericTableValuedParameter(rows, typeSqlDbType);
            _rows = genericTvp.AsEnumerable();
        }

        public Tvp(string parameterName, string typeName, IEnumerable<SqlDataRecord> rows)
        {
            _parameterName = parameterName;
            _typeName = typeName;
            _rows = rows;
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            SqlCommand sqlCommand = null;
            if (command is SqlCommand)
            {
                sqlCommand = (SqlCommand)command;
            }
            if (sqlCommand == null)
            {
                throw new ArgumentNullException($"{typeof(SqlCommand).Name}", "Could not convert to a SqlCommand.");
            }

            SqlParameter sqlParameter = sqlCommand.Parameters.Add(_parameterName, SqlDbType.Structured);
            sqlParameter.Direction = ParameterDirection.Input;
            sqlParameter.TypeName = _typeName;
            sqlParameter.Value = _rows;
        }
    }
}