#if NET461
using System;
using System.Reflection;
#else
using System;
using System.Reflection;
#endif

namespace Dapper.TableValuedParameter.Extensions
{
#if NET461
    public static class TypeExtensions
    {
        public static Assembly Assembly(this Type type)
        {
            return type.Assembly;
        }

        public static bool IsValueType(this Type type)
        {
            return type.IsValueType;
        }
    }

#else
    public static class TypeExtensions
    {
        public static Assembly Assembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }
    }
#endif
}
