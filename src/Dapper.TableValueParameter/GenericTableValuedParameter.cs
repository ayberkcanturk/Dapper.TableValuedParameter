using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

using Dapper.TableValueParameter.Mapping;

using Microsoft.SqlServer.Server;

#if NET451 || NET462
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.TableValueParameter.Extensions;
#endif

namespace Dapper.TableValueParameter
{
    internal class GenericTableValuedParameter : IEnumerable<SqlDataRecord>
    {
        private readonly IEnumerable<object> _tableValuedList;

        public GenericTableValuedParameter(IEnumerable<object> tableValuedList)
        {
            _tableValuedList = tableValuedList;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            Type type = _tableValuedList.GetType().GetGenericArguments().Single();
            PropertyInfo[] properties = type.GetProperties();
            var metaData = new SqlMetaData[properties.Length];

            for (var i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];

#if NET451 || NET462
                var columnNameAttribute = property.GetAttribute<ColumnAttribute>();
                string name = columnNameAttribute != null ? columnNameAttribute.Name : property.Name;
#endif

#if NETSTANDARD1_6
                string name = property.Name;
#endif
                SqlDbType dbType = TypeSqlDbTypeMap.GetSqlDbType(property.PropertyType);

#if NET451 || NET462
                if (dbType == SqlDbType.NVarChar)
                {
                    int length = 0;
                    var lengthAttribute = property.GetAttribute<MaxLengthAttribute>();
                    if (lengthAttribute != null)
                    {
                        length = lengthAttribute.Length;
                    }
                    metaData[i] = new SqlMetaData(name, dbType, length == default(int) ? SqlMetaData.Max : length);
                }
                else
                {
                    metaData[i] = new SqlMetaData(name, dbType);
                }
#endif

#if NETSTANDARD1_6
                if (dbType == SqlDbType.NVarChar)
                {
                    metaData[i] = new SqlMetaData(name, dbType, SqlMetaData.Max);
                }
                else
                {
                    metaData[i] = new SqlMetaData(name, dbType);
                }
#endif
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
