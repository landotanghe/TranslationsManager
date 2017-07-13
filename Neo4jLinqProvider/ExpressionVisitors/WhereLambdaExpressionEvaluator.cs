using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Translations.Data.NodeDefinitions;

namespace Neo4jLinqProvider.ExpressionVisitors
{
    public class WhereLambdaExpressionEvaluator : ExpressionVisitor
    {
        private Arguments _arguments;
        private string _where = null;

        public WhereLambdaExpressionEvaluator(Arguments arguments)
        {
            _arguments = arguments;
        }

        public string GetWhere(Expression expression)
        {
            Visit(expression);

            if (_where == null)
            {
                throw new NotSupportedException("this expression can't be used in where");
            }
            return _where;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            Console.WriteLine("binary expression node");
            var leftVisitor = new WhereLambdaExpressionEvaluator(_arguments);
            var rightVisitor = new WhereLambdaExpressionEvaluator(_arguments);
            var left = leftVisitor.GetWhere(b.Left);
            var right = leftVisitor.GetWhere(b.Right);
            string @operator = GetBinaryOperator(b);

            _where = $"{left} {@operator} {right}";

            return b;
        }

        private static string GetBinaryOperator(BinaryExpression b)
        {
            var @operator = GetLogicalOperator(b) ?? GetArithmicOperator(b);
            if(@operator == null)
                throw new NotImplementedException("operator not supported yet");
            return @operator;
        }


        private static string GetLogicalOperator(BinaryExpression b)
        {
            if (b.NodeType == ExpressionType.Equal)
            {
                return "=";
            }
            else if (b.NodeType == ExpressionType.NotEqual)
            {
                return "<>";
            }
            else if (b.NodeType == ExpressionType.GreaterThan)
            {
                return ">";
            }
            else if (b.NodeType == ExpressionType.GreaterThanOrEqual)
            {
                return ">=";
            }
            else if (b.NodeType == ExpressionType.LessThan)
            {
                return "<";
            }
            else if (b.NodeType == ExpressionType.LessThanOrEqual)
            {
                return "<=";
            }
            else if (b.NodeType == ExpressionType.OrElse)
            {
                return "OR";
            }
            else if (b.NodeType == ExpressionType.AndAlso)
            {
                return "AND";
            }
            return null;
        }

        private static string GetArithmicOperator(BinaryExpression b)
        {
            if (b.NodeType == ExpressionType.Add)
            {
                return "+";
            }
            else if (b.NodeType == ExpressionType.Subtract)
            {
                return "-";
            }
            else if (b.NodeType == ExpressionType.Multiply)
            {
                return "*";
            }
            else if (b.NodeType == ExpressionType.Divide)
            {
                return "/";
            }
            return null;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            var callVisitor = new CallToContainsVisitor(_arguments);
            _where = callVisitor.GetWhere(m);
            return m;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            var propertyAttribute = (PropertyAttribute)m.Member.GetCustomAttributes(typeof(PropertyAttribute), true).SingleOrDefault();

            if (propertyAttribute != null)
            {
                var propertyName = propertyAttribute.GetName();
                _where = "n0." + propertyName;
            }
            else
            {
                var parameter = _arguments.AddParameter(GetValue(m));
                _where = "{" + parameter + "}";
                return m;
            }
            return base.VisitMemberAccess(m);
        }


        private int parameterIndex = 0;

        protected override Expression VisitConstant(ConstantExpression c)
        {
            var parameterName = _arguments.AddParameter(c.Value);

            //TODO clean up by using separate Visitor class and call that one once for the left part and once for the right 
            //part and storing the result in the separate visitor class so it can be extracted after visiting the subtree.
            _where = "{" + parameterName + "}";
            return base.VisitConstant(c);
        }
    }
}