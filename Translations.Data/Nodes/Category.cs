using System.Collections.Generic;
using Translations.Data.NodeDefinitions;

namespace Translations.Data.Nodes
{
    [Node(label: "Category")]
    public class Category : IEntityNode
    {
        [UpstreamEdge(label: "IsPartOf")]
        public List<Word> Words { get; set; }
    }
}
