using Translations.Data.NodeDefinitions;

namespace Translations.Data.Nodes
{
    public abstract class AbstractWord : Node
    {
        [Property(name:"name")]
        public string Name { get; set; }

        [Property(name: "language")]
        public string Language { get; set; }
    }
}
