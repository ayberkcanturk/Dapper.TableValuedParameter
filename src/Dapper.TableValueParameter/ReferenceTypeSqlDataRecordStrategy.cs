using System;
using System.Reflection;
using System.Data;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

using Microsoft.SqlServer.Server;

using Dapper.TableValuedParameter.Extensions;
using Dapper.TableValuedParameter.Attributes;

namespace Dapper.TableValuedParameter
{
    public class ReferenceTypeSqlDataRecordStrategy : SqlDataRecordStrategy
    {
        public ReferenceTypeSqlDataRecordStrategy(string parameterName,
            IEnumerable<object> tableValuedList,
            Type type,
            TypeSqlDbTypeMap typeSqlDbTypeMap) : base(parameterName, tableValuedList, type, typeSqlDbTypeMap)
        {
        }

        public override IEnumerator<SqlDataRecord> GetEnumerator()
        {
            PropertyInfo[] properties = base._type.GetProperties();
            var metaData = new SqlMetaData[properties.Length];

            for (var i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];

                var columnNameAttribute = property.GetAttribute<ColumnAttribute>();
                string name = columnNameAttribute != null ? columnNameAttribute.Name : property.Name;

                var mapAttribute = property.GetAttribute<MapAttribute>();
                SqlDbType dbType = mapAttribute?.SqlDbType ?? _typeSqlDbTypeMap.GetSqlDbType(property.PropertyType);

                if (dbType == SqlDbType.NVarChar)
                {
                    var length = 0;
                    var lengthAttribute = property.GetAttribute<MaxLengthAttribute>();
                    if (lengthAttribute != null) length = lengthAttribute.Length;
                    metaData[i] = new SqlMetaData(name, dbType, length == default(int) ? SqlMetaData.Max : length);
                }
                else metaData[i] = new SqlMetaData(name, dbType);
            }

            foreach (object item in _tableValuedList)
            {
                var sqlDataRecord = new SqlDataRecord(metaData);
                try
                {
                    object[] values = properties.Select(x => x.GetValue(item, null)).ToArray();
                    sqlDataRecord.SetValues(values);
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