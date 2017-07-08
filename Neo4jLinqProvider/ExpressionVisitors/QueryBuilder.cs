using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Neo4jLinqProvider.ExpressionVisitors
{
    public class QueryBuilder : ExpressionVisitor
    {
        private Expression _expression;
        private Query _query;
        

        public QueryBuilder(Expression expression)
        {
            _expression = expression;
            _query = new Query
            {
                Body = "MATCH (myWord:Word)WHERE myWord.name IN ['koe']RETURN myWord.name, myWord.language",
                Arguments = new Dictionary<string, object>()
            };
        }

        public Query Build()
        {
            Visit(_expression);
            return _query;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            Console.WriteLine("binary node");
            return base.VisitBinary(b);
        }
    }

    public class Query
    {
        public string Body { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
    }
}
