using System;
using System.Linq;
using System.Linq.Expressions;
using Translations.Data.NodeDefinitions;

namespace Neo4jLinqProvider.ExpressionVisitors
{
    public class BinaryExpressionReduceVisitor : ExpressionVisitor
    {
        public Expression Reduce(Expression expression)
        {
            var original = new ExpressionLogVisitor().Log(expression, 250);
            var result = Visit(expression);
            var res = new ExpressionLogVisitor().Log(expression, 250);
            return result;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            //First go as deep as possible, deep nodes can be merged, making the tree go upwards
            var left = Visit(b.Left);
            var right = Visit(b.Right);

            if (IsMergable(left) && IsMergable(right))
            {
                var result = b;
                var lambda = Expression.Lambda<Func<bool>>(b);
                var value = lambda.Compile()();
                return Expression.Constant(value);
            }

            return b;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if(!m.Member.GetCustomAttributes(typeof(PropertyAttribute), true).Any())
            {
                return Expression.Constant(GetValue(m));
            }
            return m;
        }

        private bool IsMergable(Expression part)
        {
            return part.NodeType == ExpressionType.Constant;
               // (part.NodeType == ExpressionType.MemberAccess && !((MemberExpression)part).Member.GetCustomAttributes(typeof(PropertyAttribute), true).Any());
        }

        //private BinaryExpression Reduce(BinaryExpression b)
        //{
        //    var left = ExtractValue(b.Left);
        //    var right = ExtractValue(b.Right);
        //    return left + right;
        //}

        //private object ExtractValue(Expression left)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
