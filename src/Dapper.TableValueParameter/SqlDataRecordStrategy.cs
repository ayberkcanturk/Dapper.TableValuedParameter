using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.SqlServer.Server;

namespace Dapper.TableValuedParameter
{
    public abstract class SqlDataRecordStrategy : IEnumerable<SqlDataRecord>
    {
        protected readonly string _parameterName;
        protected readonly IEnumerable<object> _tableValuedList;
        protected readonly Type _type;
        protected readonly TypeSqlDbTypeMap _typeSqlDbTypeMap;

        public SqlDataRecordStrategy(string parameterName,
            IEnumerable<object> tableValuedList,
            Type type,
            TypeSqlDbTypeMap typeSqlDbTypeMap)
        {
            _parameterName = parameterName;
            _tableValuedList = tableValuedList;
            _type = type;
            _typeSqlDbTypeMap = typeSqlDbTypeMap;
        }

        public abstract IEnumerator<SqlDataRecord> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}