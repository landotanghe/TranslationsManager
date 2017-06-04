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

        private CypherMatchBuilder(string variableName)
        {
            _labels = new List<string>();
            _variableName = variableName;
            _propertyArguments = new Dictionary<string, string>();
        }

        public static CypherMatchBuilder Match(string variableName, Type type)
        {
            var matcher = new CypherMatchBuilder(variableName);
            return matcher.Match(type);
        }

        private CypherMatchBuilder Match(Type type)
        {
            var labels = type.GetCustomAttributes(true).OfType<NodeAttribute>().Select(n => n.GetLabel());
            _labels.AddRange(labels);
            _labels = _labels.Distinct().ToList();

            return this;
        }

        public CypherMatchBuilder PropertyIs<T>(Expression<Func<T, object>> memberExpression, string argumentName)
        {
            var nodeProperty = ReflectionHelpers.GetCustomAttribute<PropertyAttribute, T>(memberExpression);
            var propertyName = nodeProperty.GetName();
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
