using System.Configuration;
using System.Collections.Generic;
using Neo4j.Driver.V1;
using System;
using System.Linq;
using Translations.Data.NodeDefinitions;
using System.Collections;
using System.Linq.Expressions;
using Neo4jLinqProvider.ExpressionVisitors;

namespace Neo4jLinqProvider
{
    class Neo4jQueryContext
    {
        internal static object Execute(Expression expression, Type nodeType)
        {
            var query = new QueryBuilder(expression).Build();

            var queryResult = ExecueQuery(query.Body, query.Arguments);
            var typedList = typeof(List<>).MakeGenericType(nodeType);

            var entities = (IList) Activator.CreateInstance(typedList);
            foreach (var queryNode in queryResult)
            {
                var node = CreateNode(nodeType, queryNode);
                entities.Add(node);
            }
            return entities;
        }

        private static object CreateNode(Type nodeType, IRecord queryNode)
        {
            var node = Activator.CreateInstance(nodeType);

            var nodeProperties = nodeType.GetProperties();
            for (int i = 0; i < queryNode.Keys.Count; i++)
            {
                var queryNodeName = queryNode.Keys[i];
                var queryNodeValue = queryNode.Values[queryNodeName];

                var propertyToSet = nodeProperties
                    .Where(prop => prop.GetCustomAttributes(true).OfType<PropertyAttribute>()
                                                        .Any(att => queryNodeName == "myWord." + att.GetName()))
                    .First();
                var propertySetter = propertyToSet.SetMethod;

                propertySetter.Invoke(node, new object[] { queryNodeValue });
            }

            return node;
        }

        private static IStatementResult ExecueQuery(string query, Arguments arguments)
        {
            var uri = ConfigurationManager.AppSettings["database.url"];
            var username = ConfigurationManager.AppSettings["database.username"];
            var password = ConfigurationManager.AppSettings["database.password"];

            using (var driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password)))
            using (var session = driver.Session())
            {
                var result = session.Run(query, arguments);
                return result;
            }
        }
    }
}
