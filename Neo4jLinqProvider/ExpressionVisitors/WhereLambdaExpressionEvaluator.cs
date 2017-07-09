using System;
using System.Linq;
using System.Linq.Expressions;
using Translations.Data.NodeDefinitions;

namespace Neo4jLinqProvider.ExpressionVisitors
{
    public class WhereLambdaExpressionEvaluator : ExpressionVisitor
    {
        private Query _query;
        private string _where = null;
        private string _left = null;
        private string _right = null;

        public WhereLambdaExpressionEvaluator(Query query)
        {
            _query = query;
        }

        public string GetWhere(Expression expression)
        {
            Visit(expression);

            if (_where == null)
                throw new NotSupportedException("this expression can't be used in where");

            return _where;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            Console.WriteLine("binary expression node");
            Expression left = this.Visit(b.Left);
            Expression right = this.Visit(b.Right);

            if (b.NodeType == ExpressionType.Equal)
            {
                _where = $"{_left} = {_right}";
            }
            else if (b.NodeType == ExpressionType.NotEqual)
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
            var propertyAttribute = (PropertyAttribute)m.Member.GetCustomAttributes(typeof(PropertyAttribute), true).Single();
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
    }
}
