using System;
using System.Linq.Expressions;

namespace Neo4jLinqProvider.ExpressionVisitors
{
    public class ExpressionLogVisitor : ExpressionVisitor
    {
        private string _log = "";
        private int _maxLength;
        public string Log(Expression exp, int maxLength)
        {
            _maxLength = maxLength;
            Visit(exp);
            return _log;
        }

        protected override Expression VisitUnary(UnaryExpression expression)
        {
            return Report(base.VisitUnary, expression);
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {
            return Report(base.VisitBinary, expression);
        }

        protected override Expression VisitTypeIs(TypeBinaryExpression expression)
        {
            return Report(base.VisitTypeIs, expression);
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            return Report(base.VisitConstant, expression);
        }

        protected override Expression VisitConditional(ConditionalExpression expression)
        {
            return Report(base.VisitConditional, expression);
        }

        protected override Expression VisitParameter(ParameterExpression expression)
        {
            return Report(base.VisitParameter, expression);
        }

        protected override Expression VisitMemberAccess(MemberExpression expression)
        {
            return Report(base.VisitMemberAccess, expression);
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            return Report(base.VisitMethodCall, expression);
        }

        protected override Expression VisitLambda(LambdaExpression expression)
        {
            return Report(base.VisitLambda, expression);
        }

        protected override NewExpression VisitNew(NewExpression expression)
        {
            return Report2(base.VisitNew, expression);
        }

        protected override Expression VisitMemberInit(MemberInitExpression expression)
        {
            return Report(base.VisitMemberInit, expression);
        }

        protected override Expression VisitListInit(ListInitExpression expression)
        {
            return Report(base.VisitListInit, expression);
        }

        protected override Expression VisitNewArray(NewArrayExpression expression)
        {
            return Report(base.VisitNewArray, expression);
        }

        private int _depth = 0;
        private Expression Report<T>(Func<T, Expression> handle, T expression) where T : Expression
        {
            _depth++;
            var stringified = expression.ToString();
            if(stringified.Length > _maxLength)
            {
                stringified = stringified.Substring(0, _maxLength);
            }
            _log += $"{_depth.Tabs()} {expression.NodeType} {stringified}" + "\r\n";
            var result = handle(expression);
            _depth--;
            return result;
        }

        private T Report2<T>(Func<T, T> handle, T expression) where T : Expression
        {
            _depth++;
            var stringified = expression.ToString();
            if (stringified.Length > _maxLength)
            {
                stringified = stringified.Substring(0, _maxLength);
            }
            _log += $"{_depth.Tabs()} {expression.NodeType} {stringified}" + "\r\n";
            var result = handle(expression);
            _depth--;
            return result;
        }
    }

    public static class ExpressionLogVisitorHelper
    {
        public static string Tabs(this int tabCount)
        {
            string tabs = "";
            for(int i = 0; i < tabCount; i++)
            {
                tabs += "\t";
            }
            return tabs;
        }
    }
}
