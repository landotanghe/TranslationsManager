using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Translations.Data.NodeDefinitions;

namespace Neo4jLinqProvider.ExpressionVisitors
{
    public class CallToContainsVisitor : ExpressionVisitor
    {
        private Arguments _arguments;
        private string _where = null;

        public CallToContainsVisitor(Arguments arguments)
        {
            _arguments = arguments;
        }

        private string _variableToContain;
        private List<string> _listToContain;

        public string GetWhere(Expression expression)
        {
            Visit(expression);

            if (_where == null)
            {
                throw new NotSupportedException("this expression can't be used in where");
            }
            return _where;
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
            else if (GetValue(m) is object[])
            {
                var values = (object[])GetValue(m);
                _listToContain = values.Select(v => v.ToString()).ToList();
                return m;
            }

            if (_variableToContain != null && _listToContain != null)
            {
                string parameters = String.Join(",", _listToContain
                    .Select(value => _arguments.AddParameter(value))
                    .Select(param => "{" + param + "}"));

                _where = $"{_variableToContain} IN [{parameters}]";
            }

            return base.VisitMemberAccess(m);
        }

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
