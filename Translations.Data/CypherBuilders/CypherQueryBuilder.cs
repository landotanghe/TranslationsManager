using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Translations.Data.NodeDefinitions;

namespace Translations.Data.CypherBuilders
{
    public class CypherQueryBuilder<T> where T : IEntityNode
    {
        private List<CypherMatchBuilder> _matchers;
        private CypherReturnBuilder _returner;
        private CypherArgumentBuilder _argumentBuilder;

        public CypherQueryBuilder()
        {
            _argumentBuilder = new CypherArgumentBuilder();
            _matchers = new List<CypherMatchBuilder>();
            _returner = CypherReturnBuilder.Create();
        }
        
        public CypherQueryBuilder<T> Match(Expression<Func<T,bool>> whereExpression) 
        {
            var variableName = "myWord";

            var matcher = CypherMatchBuilder
                .Match<T>(variableName, _argumentBuilder)
                .Where(whereExpression);

            _matchers.Add(matcher);
            return this;
        }

        public List<T> ToList()
        {
            var queryResult = GetQueryResult();
            var entities = new List<T>();
            foreach(var node in queryResult)
            {
                entities.Add(_returner.FillKnownProperties<T>(node));
            }
            return entities;
        }

        public T FirstOrDefault()
        {
            var queryResult = GetQueryResult();
            var firstNode = queryResult.FirstOrDefault();
            if (firstNode == null)
                return default(T);

            var resultEntity = _returner.FillKnownProperties<T>(firstNode);
            return resultEntity;
        }

        private IStatementResult GetQueryResult()
        {
            var variableName = "myWord";
            _returner.ReturnEntireClass<T>(variableName);

            string matchQuery = String.Join(" ", _matchers.Select(m => m.ToString()));
            string returnQuery = _returner.ToString();

            var query = $"{matchQuery}{returnQuery}";
            var queryResult = ExecueQuery(query, _argumentBuilder.GetArguments());
            return queryResult;
        }

        private IStatementResult ExecueQuery(string query, params Argument[] arguments)
        {
            var dictionary = arguments.ToDictionary(arg => arg.Name, arg => arg.Value);
            return ExecueQuery(query, dictionary);
        }

        private IStatementResult ExecueQuery(string query, Dictionary<string, object> arguments)
        {
            using (var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "test")))
            using (var session = driver.Session())
            {
                var result = session.Run(query, arguments);
                return result;
            }
        }
    }
}
