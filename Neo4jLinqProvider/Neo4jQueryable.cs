using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Neo4jLinqProvider
{
    public class Neo4jQueryable<TData> : IOrderedQueryable<TData>
    {
        public Neo4jQueryable()
        {
            Provider = new Neo4jLinqProvider();
            Expression = Expression.Constant(this);
        }

        public Neo4jQueryable(Neo4jLinqProvider provider, Expression expression)
        {
            Validate(provider);
            Validate(expression);

            Provider = provider;
            Expression = expression;
        }
        
        private static void Validate(Neo4jLinqProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
        }

        private static void Validate(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (!typeof(IQueryable<TData>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }
        }

        public Type ElementType => typeof(TData);

        public Expression Expression { get; private set; }

        public IQueryProvider Provider { get; private set; }


        #region Enumerators
        public IEnumerator<TData> GetEnumerator()
        {
            return (Provider.Execute<IEnumerable<TData>>(Expression)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Provider.Execute<System.Collections.IEnumerable>(Expression)).GetEnumerator();
        }
        #endregion
    }
}
