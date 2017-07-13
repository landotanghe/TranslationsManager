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
            var _left = leftVisitor.GetWhere(b.Left);
            var _right = leftVisitor.GetWhere(b.Right);
            string _operator = GetBinaryOperator(b);

            _where = $"{_left} {_operator} {_right}";

            return b;
        }

        private static string GetBinaryOperator(BinaryExpression b)
        {
            string _operator = null;

            if (b.NodeType == ExpressionType.Equal)
            {
                _operator = "=";
            }
            else if (b.NodeType == ExpressionType.NotEqual)
            {
                _operator = "<>";
            }
            else if (b.NodeType == ExpressionType.GreaterThan)
            {
                _operator = ">";
            }
            else if (b.NodeType == ExpressionType.GreaterThanOrEqual)
            {
                _operator = ">=";
            }
            else if (b.NodeType == ExpressionType.LessThan)
            {
                _operator = "<";
            }
            else if (b.NodeType == ExpressionType.LessThanOrEqual)
            {
                _operator = "<=";
            }
            else if (b.NodeType == ExpressionType.OrElse)
            {
                _operator = "OR";
            }
            else if (b.NodeType == ExpressionType.AndAlso)
            {
                _operator = "AND";
            }
            else
            {
                throw new NotImplementedException("operator not supported yet");
            }

            return _operator;
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