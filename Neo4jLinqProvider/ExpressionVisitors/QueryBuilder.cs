using System;
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

        private string _where = "myWord.name IN ['koe']";
        private string _left = null;
        private string _right = null;

        protected override Expression VisitBinary(BinaryExpression b)
        {
            Console.WriteLine("binary expression node");
            Expression left = this.Visit(b.Left);
            Expression right = this.Visit(b.Right);

            if(b.NodeType == ExpressionType.Equal)
            {
                _where = $"{_left} = {_right}";
            }else if(b.NodeType == ExpressionType.NotEqual)
            {
                _where = $"{_left} <> {_right}";
            }
            else
            {
                throw new NotImplementedException("operator not supported yet");
            }

            return b;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            var propertyAttribute = (PropertyAttribute) m.Member.GetCustomAttributes(typeof(PropertyAttribute), true).Single();
            var propertyName = propertyAttribute.GetName();
            //TODO clean up by using separate Visitor class and call that one once for the left part and once for the right 
            //part and storing the result in the separate visitor class so it can be extracted after visiting the subtree.
            if (_left == null)
            {
                _left = "myWord." + propertyName;
            }
            else
            {
                _right = "myWord." + propertyName;
            }

            return base.VisitMemberAccess(m);
        }

        private int parameterIndex = 0;

        protected override Expression VisitConstant(ConstantExpression c)
        {
            var parameterName = "P" + parameterIndex++;
            _query.Arguments.Add(parameterName, c.Value.ToString());

            //TODO clean up by using separate Visitor class and call that one once for the left part and once for the right 
            //part and storing the result in the separate visitor class so it can be extracted after visiting the subtree.
            if (_left == null)
            {
                _left = "{" + parameterName + "}";
            }
            else
            {
                _right = "{" + parameterName + "}";
            }
            return base.VisitConstant(c);
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
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
