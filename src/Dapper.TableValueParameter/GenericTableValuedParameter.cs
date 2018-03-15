using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.SqlServer.Server;

using Dapper.TableValuedParameter.Extensions;

namespace Dapper.TableValuedParameter
{
    internal class GenericTableValuedParameter
    {
        private readonly string _parameterName;
        private readonly IEnumerable<object> _tableValuedList;
        private readonly TypeSqlDbTypeMap _typeSqlDbTypeMap;

        public GenericTableValuedParameter(
            string parameterName,
            IEnumerable<object> tableValuedList,
            TypeSqlDbTypeMap typeSqlDbTypeMap)
        {
            _parameterName = parameterName;
            _tableValuedList = tableValuedList;
            _typeSqlDbTypeMap = typeSqlDbTypeMap;
        }

        public IEnumerable<SqlDataRecord> GetParameter()
        {
            Type type = _tableValuedList.GetType().GetGenericArguments().Single();
            SqlDataRecordStrategy sqlDataRecordStrategy;

            if (type.IsValueType())
            {
                sqlDataRecordStrategy = new ValueTypeSqlDataRecordStrategy(_parameterName, _tableValuedList, type, _typeSqlDbTypeMap);
            }
            else
            {
                sqlDataRecordStrategy = new ReferenceTypeSqlDataRecordStrategy(_parameterName, _tableValuedList, type, _typeSqlDbTypeMap);
            }

            return sqlDataRecordStrategy.GetEnumerator() as IEnumerable<SqlDataRecord>;
        }
    }
}
