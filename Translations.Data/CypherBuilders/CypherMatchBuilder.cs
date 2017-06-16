using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Translations.Data.NodeDefinitions;

namespace Translations.Data.CypherBuilders
{
    class CypherMatchBuilder
    {
        private string _variableName;
        private List<string> _labels;
        private Dictionary<string, string> _propertyArguments;
        private CypherArgumentBuilder _argumentBuilder;

        private CypherMatchBuilder(string variableName, CypherArgumentBuilder argumentNameBuilder)
        {
            _labels = new List<string>();
            _variableName = variableName;
            _argumentBuilder = argumentNameBuilder;
            _propertyArguments = new Dictionary<string, string>();
        }

        public static CypherMatchBuilder Match<T>(string variableName, CypherArgumentBuilder argumentNameBuilder) where T : IEntityNode
        {
            var matcher = new CypherMatchBuilder(variableName, argumentNameBuilder);
            var type = typeof(T);
            return matcher.Match(type);
        }

        private CypherMatchBuilder Match(Type type)
        {
            var labels = type.GetCustomAttributes(true).OfType<NodeAttribute>().Select(n => n.GetLabel());
            _labels.AddRange(labels);
            _labels = _labels.Distinct().ToList();

            return this;
        }

        /// Property should have the value indicated by the argument
        public CypherMatchBuilder PropertyIs<T>(Expression<Func<T, object>> memberExpression, string argumentName)
        {
            var nodeProperty = ReflectionHelpers.GetCustomAttributeForMember<PropertyAttribute, T>(memberExpression);
            var propertyName = nodeProperty.GetName();
            _propertyArguments.Add(propertyName, argumentName);

            return this;
        }

        /// Property should comply with the simple boolean expression
        /// right part should be the value, left part the argument
        public CypherMatchBuilder Where<T>(Expression<Func<T, bool>> whereExpression)
        {
            var binaryExpression = ((BinaryExpression)whereExpression.Body);
            var nodeProperty = ReflectionHelpers.GetCustomAttributeForBoolean<PropertyAttribute, T>(whereExpression);
            var propertyName = nodeProperty.GetName();

            var valueExpression = (MemberExpression) binaryExpression.Right;
            
            var argumentName = _argumentBuilder.GetNextArgumentName();
            _argumentBuilder.SetValue(argumentName, valueExpression.GetValue().ToString());
            _propertyArguments.Add(propertyName, argumentName);

            return this;
        }

        public string ToString()
        {
            var labelsFilter = String.Join("", _labels.Select(label => $":{label}"));
            var propertiesFilter = _propertyArguments.Any() ? "{" + String.Join(",", _propertyArguments.Select(pv => pv.Key + ":{" + pv.Value + "}")) + "}" : String.Empty;

            return "MATCH (" + _variableName + labelsFilter + propertiesFilter + ")";
        }
    }
}
