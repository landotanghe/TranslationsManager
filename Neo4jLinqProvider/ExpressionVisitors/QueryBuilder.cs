using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Neo4jLinqProvider.ExpressionVisitors
{
    public class QueryBuilder : ExpressionVisitor
    {
        private Expression _expression;
        private Query _query;
        private string _where;
        
        public QueryBuilder(Expression expression)
        {
            _expression = expression;
            _query = new Query
            {
                Body = null,
                Arguments = new Arguments()
            };
        }

        public Query Build()
        {
            Visit(_expression);
            _query.Body = $"MATCH (myWord:Word) WHERE {_where} RETURN myWord.name, myWord.language";
            return _query;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Arguments.Count == 2 && m.Method.DeclaringType == typeof(System.Linq.Queryable) && m.Method.Name == "Where") { 
                var whereExpression = m.Arguments[1];
                _where = (new WhereLambdaExpressionEvaluator(_query.Arguments)).GetWhere(whereExpression);
            }
            Console.WriteLine("method call expression node");
            return base.VisitMethodCall(m);
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            Console.WriteLine("unary expression node");
            return base.VisitUnary(u);
        }

        protected override Expression VisitLambda(LambdaExpression lambda)
        {
            Console.WriteLine("lambda expression node");
            return base.VisitLambda(lambda);
        }
    }

    public class Query
    {
        public string Body { get; set; }
        public Arguments Arguments { get; set; }
    }

    public class Arguments
    {
        private int _parameterIndex = 0;
        private Dictionary<string, object> _argumentsMap = new Dictionary<string, object>();

        /// <summary>
        /// Adds a parameter with the given value and a automatically determined name
        /// </summary>
        /// <param name="value">name assigned the added parameter</param>
        /// <returns></returns>
        public string AddParameter(object value)
        {
            var parameterName = "P" + _parameterIndex++;
            _argumentsMap.Add(parameterName, value.ToString());
            return parameterName;
        }

        public object this[string index]
        {
            get
            {
                return _argumentsMap[index];
            }
        }

        public int Count
        {
            get
            {
                return _argumentsMap.Count;
            }
        }
    }
}
