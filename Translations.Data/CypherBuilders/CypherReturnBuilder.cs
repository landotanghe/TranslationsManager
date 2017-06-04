using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Translations.Data.NodeDefinitions;

namespace Translations.Data.CypherBuilders
{
    class CypherReturnBuilder
    {
        private List<string> _propertiesToReturn;

        private CypherReturnBuilder()
        {
            _propertiesToReturn = new List<string>();
        }

        public static CypherReturnBuilder Create()
        {
            return new CypherReturnBuilder();
        }

        public CypherReturnBuilder ReturnEntireClass<T>(string variableName, Type type)
        {
            throw new NotImplementedException();
        }

        public CypherReturnBuilder AddMember<T>(string variableName, Expression<Func<T, object>> memberExpression)
        {
            var nodeProperty = ReflectionHelpers.GetCustomAttributeForMember<PropertyAttribute, T>(memberExpression);
            var propertyName = nodeProperty.GetName();
            _propertiesToReturn.Add($"{variableName}.{propertyName}");

            return this;
        }

        public string ToString()
        {
            var propertiesFilter = String.Join(",", _propertiesToReturn);

            return "RETURN " + propertiesFilter;
        }

    }
}
