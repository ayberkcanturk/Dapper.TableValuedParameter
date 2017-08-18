using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;

using Dapper.TableValuedParameter.Attributes;
using Dapper.TableValuedParameter.Extensions;

using Microsoft.SqlServer.Server;

namespace Dapper.TableValuedParameter
{
    internal class GenericTableValuedParameter : IEnumerable<SqlDataRecord>
    {
        private readonly IEnumerable<object> _tableValuedList;
        private readonly TypeSqlDbTypeMap _typeSqlDbTypeMap;
        private readonly string _parameterName;

        public GenericTableValuedParameter(
            string parameterName,
            IEnumerable<object> tableValuedList,
            TypeSqlDbTypeMap typeSqlDbTypeMap)
        {
            _parameterName = parameterName;
            _tableValuedList = tableValuedList;
            _typeSqlDbTypeMap = typeSqlDbTypeMap;
        }

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            Type type = _tableValuedList.GetType().GetGenericArguments().Single();

            if (type.IsValueType())
            {
                #region ValueType
                var metaData = new SqlMetaData[1]
                {
                    new SqlMetaData(_parameterName, _typeSqlDbTypeMap.GetSqlDbType(type))
                };

                foreach (var item in _tableValuedList)
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
                #endregion
            }
            else
            {
                #region Reference Type
                PropertyInfo[] properties = type.GetProperties();
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
                #endregion
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}