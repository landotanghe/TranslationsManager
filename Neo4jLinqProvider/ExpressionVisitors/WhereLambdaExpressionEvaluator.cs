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
                if (_listToContain != null)
                {
                    string parameters = String.Join(",", _listToContain
                        .Select(value => _arguments.AddParameter(value))
                        .Select(param => "{" + param + "}"));
                    _where = $"{_variableToContain} IN [{parameters}]";
                }
                else
                {
                    throw new NotSupportedException("this expression can't be used in where");
                }
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

            if (b.NodeType == ExpressionType.Equal)
            {
                _where = $"{_left} = {_right}";
            }
            else if (b.NodeType == ExpressionType.NotEqual)
            {
                _where = $"{_left} <> {_right}";
            }
            else if (b.NodeType == ExpressionType.GreaterThan)
            {
                _where = $"{_left} > {_right}";
            }
            else if (b.NodeType == ExpressionType.GreaterThanOrEqual)
            {
                _where = $"{_left} >= {_right}";
            }
            else if (b.NodeType == ExpressionType.LessThan)
            {
                _where = $"{_left} < {_right}";
            }
            else if (b.NodeType == ExpressionType.LessThanOrEqual)
            {
                _where = $"{_left} <= {_right}";
            }
            else
            {
                throw new NotImplementedException("operator not supported yet");
            }

            return b;
        }

        private string _variableToContain;
        private List<string> _listToContain;

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            //if (m.Method.DeclaringType == typeof(Enumerable) && m.Method.Name == "Contains")
            //{
            //    _variableToContain = m.Arguments[1];
            //}
            Console.WriteLine("method call expression node");
            return base.VisitMethodCall(m);
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            var propertyAttribute = (PropertyAttribute)m.Member.GetCustomAttributes(typeof(PropertyAttribute), true).SingleOrDefault();

            if (propertyAttribute != null)
            {
                var propertyName = propertyAttribute.GetName();
                _where = "n0." + propertyName;
                _variableToContain = "n0." + propertyName;
            }
            else
            {
                var values = (object[])GetValue(m);
                _listToContain = values.Select(v => v.ToString()).ToList();
            }

            return base.VisitMemberAccess(m);
        }

        private object GetValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();

            return getter();
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