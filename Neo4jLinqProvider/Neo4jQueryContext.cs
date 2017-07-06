using System.Configuration;
using System.Collections.Generic;
using Neo4j.Driver.V1;

namespace Neo4jLinqProvider
{
    class Neo4jQueryContext
    {
        internal static object Execute(System.Linq.Expressions.Expression expression, bool IsEnumerable)
        {
            var result = ExecueQuery("Match(n) Return n", new Dictionary<string, object>());
            return result;
        }

        private static IStatementResult ExecueQuery(string query, Dictionary<string, object> arguments)
        {
            var uri = ConfigurationManager.AppSettings["database.url"];
            var username = ConfigurationManager.AppSettings["database.username"];
            var password = ConfigurationManager.AppSettings["database.password"];

            using (var driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password)))
            using (var session = driver.Session())
            {
                var result = session.Run(query, arguments);
                return result;
            }
        }
    }
}
