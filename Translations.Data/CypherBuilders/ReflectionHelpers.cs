using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Translations.Data.CypherBuilders
{
    public static class ReflectionHelpers
    {
        public static Attr GetCustomAttribute<Attr, T>(Expression<Func<T, object>> memberExpression) where Attr : Attribute
        {
            var member = (MemberExpression)memberExpression.Body;
            var customAttribute = (Attr) member.Member.GetCustomAttribute(typeof(Attr));
            return customAttribute;
        }
    }
}
