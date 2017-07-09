﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Translations.Data.NodeDefinitions;

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
                Arguments = new Dictionary<string, object>()
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
            if (m.Arguments.Count == 2) { 
                var whereExpression = m.Arguments[1];
                _where = (new WhereLambdaExpressionEvaluator(_query)).GetWhere(whereExpression);
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
        public Dictionary<string, object> Arguments { get; set; }
    }
}
