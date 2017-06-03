using System.Collections.Generic;
using Translations.Data.NodeDefinitions;

namespace Translations.Data.Nodes
{
    [Node(label: "Word")]
    public class Word : AbstractWord
    {
        [DownstreamEdge(label: "IsPartOf")]
        public Category Category{ get; set; }
        
        [UpstreamEdge(label: "IsTranslationOf")]
        public List<Translation> Translations { get; set; }
    }
}
