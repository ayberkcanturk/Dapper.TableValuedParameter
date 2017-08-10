using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Dapper.TableValueParameter;

using Microsoft.SqlServer.Server;

namespace Dapper
{
    public class TableValuedParameter : SqlMapper.IDynamicParameters
    {
        private readonly string _parameterName;
        private readonly IEnumerable<SqlDataRecord> _rows;
        private readonly string _typeName;

        public TableValuedParameter(string parameterName, string typeName, IEnumerable<object> rows)
        {
            _parameterName = parameterName;
            _typeName = typeName;

            var genericTvp = new GenericTableValuedParameter(rows);
            _rows = genericTvp;
        }

        public TableValuedParameter(string parameterName, string typeName, IEnumerable<SqlDataRecord> rows)
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
                throw new ArgumentException("Could not convert to a SqlCommand.", $"{typeof(SqlCommand).Name}");
            }

            SqlParameter sqlParameter = sqlCommand.Parameters.Add(_parameterName, SqlDbType.Structured);
            sqlParameter.Direction = ParameterDirection.Input;
            sqlParameter.TypeName = _typeName;
            sqlParameter.Value = _rows;
        }
    }
}
