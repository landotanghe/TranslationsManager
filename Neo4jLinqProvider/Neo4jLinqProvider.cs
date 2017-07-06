using System;
using System.Linq;
using System.Linq.Expressions;

namespace Neo4jLinqProvider
{
    public class Neo4jLinqProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                var genericType = typeof(Neo4jQueryable<>).MakeGenericType(elementType);
                return (IQueryable) Activator.CreateInstance(genericType, new object[] { this, expression });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return new Neo4jQueryable<TResult>(this, expression);
        }

        public object Execute(Expression expression)
        {
            return Neo4jQueryContext.Execute(expression, false);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            bool IsEnumerable = (typeof(TResult).Name == "IEnumerable`1");

            return (TResult)Neo4jQueryContext.Execute(expression, IsEnumerable);
        }
    }
}
