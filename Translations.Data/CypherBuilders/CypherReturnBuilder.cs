using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Translations.Data.NodeDefinitions;

namespace Translations.Data.CypherBuilders
{
    class CypherReturnBuilder
    {
        private List<string> _propertiesToReturn;
        private List<MethodInfo> _setters;

        private CypherReturnBuilder()
        {
            _propertiesToReturn = new List<string>();
            _setters = new List<MethodInfo>();
        }

        public static CypherReturnBuilder Create()
        {
            return new CypherReturnBuilder();
        }

        public void ReturnEntireClass<T>(string variableName)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                AddMember(variableName, property);
            }
        }
        
        public void AddMember(string variableName, PropertyInfo property)
        {
            var propertyAttributes = property.GetCustomAttributes(typeof(PropertyAttribute), true);
            if (propertyAttributes.Length == 0)
                return;
            if (propertyAttributes.Length > 1)
            {
                throw new ArgumentException($"Exactly 1 PropertyAttribute expected for property {property.Name} in {property.DeclaringType}");
            }
            var propertyAttribute = (PropertyAttribute)propertyAttributes[0];
            var propertyName = propertyAttribute.GetName();

            var setter = property.GetSetMethod();
            _setters.Add(setter);

            _propertiesToReturn.Add($"{variableName}.{propertyName}");
        }

        public T FillKnownProperties<T>(IRecord record)
        {
            var resultEntity = Activator.CreateInstance<T>();
            
            foreach (var setter in _setters)
            {
                var index = _setters.IndexOf(setter);
                setter.Invoke(resultEntity, new object[] { record[index] });
            }
            return resultEntity;
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
