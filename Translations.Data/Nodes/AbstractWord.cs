using Translations.Data.NodeDefinitions;

namespace Translations.Data.Nodes
{
    public abstract class AbstractWord : IEntityNode
    {
        [Property(name:"name")]
        public string Name { get; set; }

        [Property(name: "language")]
        public string Language { get; set; }
    }
}
