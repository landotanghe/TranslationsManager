using System;
using System.Collections;
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
        private CypherArgumentBuilder _argumentBuilder;
        private PossibleValues _possibleValues;

        private class PossibleValues
        {
            private Dictionary<string, List<string>> _propertyArguments;

            public PossibleValues()
            {
                _propertyArguments = new Dictionary<string, List<string>>();
            }

            public void Add(string property, string possibleValue)
            {
                if (!_propertyArguments.ContainsKey(property))
                {
                    _propertyArguments.Add(property, new List<string>());
                }
                _propertyArguments[property].Add(possibleValue);
            }

            public IEnumerable<string> Properties => _propertyArguments.Keys;
            public List<string> this[string property]
            {
                get{ return _propertyArguments[property]; }
            }
        }

        private CypherMatchBuilder(string variableName, CypherArgumentBuilder argumentNameBuilder)
        {
            _labels = new List<string>();
            _variableName = variableName;
            _argumentBuilder = argumentNameBuilder;
            _possibleValues = new PossibleValues();
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
            _possibleValues.Add(propertyName, argumentName);

            return this;
        }

        /// Property should comply with the simple boolean expression
        /// right part should be the value, left part the argument
        public CypherMatchBuilder Where<T>(Expression<Func<T, bool>> whereExpression)
        {
            if(whereExpression.Body is BinaryExpression)
            {
                var binaryExpression = ((BinaryExpression)whereExpression.Body);
                var nodeProperty = ReflectionHelpers.GetCustomAttributeForBoolean<PropertyAttribute, T>(whereExpression);
                var propertyName = nodeProperty.GetName();

                var valueExpression = (MemberExpression)binaryExpression.Right;

                var argumentName = _argumentBuilder.GetNextArgumentName();
                _argumentBuilder.SetValue(argumentName, valueExpression.GetValue().ToString());
                _possibleValues.Add(propertyName, argumentName);
            }
            else if(whereExpression.Body is MethodCallExpression)
            {
                var methodCallExpression = ((MethodCallExpression)whereExpression.Body);
                var method = methodCallExpression.Method;
                if ((typeof(IEnumerable)).IsAssignableFrom(method.DeclaringType)){
                    var argumentToContain = methodCallExpression.Arguments[0];
                    var nodeProperty = ReflectionHelpers.GetCustomAttributeForMember<PropertyAttribute, T>((MemberExpression)argumentToContain);
                    var propertyName = nodeProperty.GetName();

                    var target= ((MemberExpression)methodCallExpression.Object).GetValue();
                    var possibleValues = (IEnumerable<string>)target;
                    foreach(var possibleValue in possibleValues)
                    {
                        var argumentName = _argumentBuilder.GetNextArgumentName();
                        _argumentBuilder.SetValue(argumentName, possibleValue);
                        _possibleValues.Add(propertyName, argumentName);
                    }
                }
            }


            return this;
        }

        public string ToString()
        {
            var labelsFilter = String.Join("", _labels.Select(label => $":{label}"));
            var whereFilter = _possibleValues.Properties.Any() ? "WHERE " : String.Empty;
            whereFilter += String.Join(" AND ", _possibleValues.Properties
                                                                .Select(CreateFilterForProperty));
            return "MATCH (" + _variableName + labelsFilter + ")" + whereFilter;

        }

        private string CreateFilterForProperty(string property)
        {
            var args = String.Join(",", _possibleValues[property].Select(argName => "{" + argName + "}"));
            return $"{_variableName}.{property} IN [{args}]";
        }
    }
}
