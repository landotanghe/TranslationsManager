using System;
using System.Collections.Generic;
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
            Type elementType = TypeSystem.GetElementType(expression.Type);
            return Neo4jQueryContext.Execute(expression, elementType);
        }

        // Queryable's "single value" standard query operators call this method.
        // It is also called from QueryableTerraServerData.GetEnumerator(). 
        public TResult Execute<TResult>(Expression expression)
        {
            var type = typeof(TResult);
            bool IsEnumerable = (type.Name == "IEnumerable`1");
            
            Type elementType = IsEnumerable ? type.GetGenericArguments()[0] : type;
            var nodes = Neo4jQueryContext.Execute(expression, elementType);

            if (IsEnumerable)
            {
                return (TResult)nodes;
            }else
            {
                return ((IEnumerable<TResult>)nodes).FirstOrDefault();
            }
        }
    }
}
