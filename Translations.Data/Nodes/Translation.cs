using Translations.Data.NodeDefinitions;

namespace Translations.Data.Nodes
{
    [Node(label:"Translation")]
    public class Translation : AbstractWord
    {
        [DownstreamEdge(label: "IsTranslationOf")]
        public Word Word { get; set; }
    }
}
