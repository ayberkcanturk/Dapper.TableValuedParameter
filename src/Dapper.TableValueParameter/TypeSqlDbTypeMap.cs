using System;
using System.Collections.Generic;
using System.Data;

namespace Dapper.TableValuedParameter
{
    public class TypeSqlDbTypeMap
    {
        private readonly IDictionary<Type, SqlDbType> _typeSqlDbTypeMap;

        public TypeSqlDbTypeMap(IDictionary<Type, SqlDbType> typeSqlDbTypeMap)
        {
            _typeSqlDbTypeMap = typeSqlDbTypeMap;
        }

        public TypeSqlDbTypeMap()
        {
            _typeSqlDbTypeMap = new Dictionary<Type, SqlDbType>
            {
                [typeof(string)] = SqlDbType.NVarChar,
                [typeof(char[])] = SqlDbType.NChar,
                [typeof(int)] = SqlDbType.Int,
                [typeof(long)] = SqlDbType.BigInt,
                [typeof(decimal)] = SqlDbType.Decimal,
                [typeof(byte)] = SqlDbType.TinyInt,
                [typeof(short)] = SqlDbType.SmallInt,
                [typeof(byte[])] = SqlDbType.Image,
                [typeof(bool)] = SqlDbType.Bit,
                [typeof(DateTime)] = SqlDbType.DateTime2,
                [typeof(DateTimeOffset)] = SqlDbType.DateTimeOffset,
                [typeof(float)] = SqlDbType.Real,
                [typeof(double)] = SqlDbType.Float,
                [typeof(TimeSpan)] = SqlDbType.Time
            };
        }

        public virtual SqlDbType GetSqlDbType(Type givenType)
        {
            givenType = Nullable.GetUnderlyingType(givenType) ?? givenType;
            if (_typeSqlDbTypeMap.ContainsKey(givenType)) return _typeSqlDbTypeMap[givenType];

            throw new ArgumentException($"{givenType.FullName} is not a supported .NET class");
        }

        public virtual SqlDbType GetSqlDbType<T>()
        {
            return GetSqlDbType(typeof(T));
        }
    }
}
