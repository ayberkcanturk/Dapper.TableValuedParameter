using System;
using System.Collections.Generic;

using Microsoft.SqlServer.Server;

namespace Dapper.TableValuedParameter
{
    public class ValueTypeSqlDataRecordStrategy : SqlDataRecordStrategy
    {
        public ValueTypeSqlDataRecordStrategy(string parameterName,
            IEnumerable<object> tableValuedList,
            Type type,
            TypeSqlDbTypeMap typeSqlDbTypeMap) : base(parameterName, tableValuedList, type, typeSqlDbTypeMap)
        {
        }
        public override IEnumerator<SqlDataRecord> GetEnumerator()
        {
            var metaData = new SqlMetaData[1]
            {
                 new SqlMetaData(base._parameterName,base._typeSqlDbTypeMap.GetSqlDbType(base._type))
            };

            foreach (object item in _tableValuedList)
            {
                var sqlDataRecord = new SqlDataRecord(metaData);
                try
                {
                    sqlDataRecord.SetValues(item);
                }
                catch (Exception exception)
                {
                    throw new ArgumentException("An error occured while setting SqlDbValues.", exception);
                }

                yield return sqlDataRecord;
            }
        }
    }
}