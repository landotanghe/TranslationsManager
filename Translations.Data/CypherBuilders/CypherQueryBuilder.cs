using System.Collections.Generic;
using Translations.Data.NodeDefinitions;

namespace Translations.Data.CypherBuilders
{
    public class CypherQueryBuilder
    {
        private List<CypherMatchBuilder> _matchers;
        private CypherReturnBuilder _returner;
        private CypherArgumentBuilder _argumentBuilder;

        public CypherQueryBuilder()
        {
            _argumentBuilder = new CypherArgumentBuilder();
            _matchers = new List<CypherMatchBuilder>();
            _returner = CypherReturnBuilder.Create();
        }

        public CypherQueryBuilder Match<T>() where T : INode
        {
          //  _matchers.Add()
            return this;
        }
    }
}
