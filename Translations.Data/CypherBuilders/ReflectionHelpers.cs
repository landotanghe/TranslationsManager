using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Translations.Data.CypherBuilders
{
    public static class ReflectionHelpers
    {
        // Retrieve the attribute added to the property
        public static Attr GetCustomAttributeForMember<Attr, T>(Expression<Func<T, object>> memberExpression) where Attr : Attribute
        {
            var member = (MemberExpression)memberExpression.Body;
            var customAttribute = (Attr) member.Member.GetCustomAttribute(typeof(Attr));
            return customAttribute;
        }

        public static Attr GetCustomAttributeForBoolean<Attr, T>(Expression<Func<T, bool>> booleanExpression) where Attr : Attribute
        {
            var binExpression = ((BinaryExpression)booleanExpression.Body);
            var member = (MemberExpression)binExpression.Left;
            var customAttribute = (Attr)member.Member.GetCustomAttribute(typeof(Attr));
            return customAttribute;
        }

        public static object GetValue(this MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));

            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            var getter = getterLambda.Compile();

            return getter();
        }
    }
}
