using System;
using System.Data;

namespace Dapper.TableValuedParameter.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class MapAttribute : Attribute
    {
        public SqlDbType SqlDbType;

        public MapAttribute(SqlDbType sqlDbType)
        {
            SqlDbType = sqlDbType;
        }
    }
}
