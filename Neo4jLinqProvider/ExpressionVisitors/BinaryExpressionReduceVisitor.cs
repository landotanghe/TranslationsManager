using System.Linq.Expressions;

namespace Neo4jLinqProvider.ExpressionVisitors
{
    public class BinaryExpressionReduceVisitor : ExpressionVisitor
    {
        public Expression Reduce(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            return base.VisitBinary(b);
        }
    }
}
