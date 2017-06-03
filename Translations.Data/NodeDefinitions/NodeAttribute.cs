using System;

namespace Translations.Data.NodeDefinitions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public class NodeAttribute : Attribute
    {
        private string _label;

        public NodeAttribute(string label)
        {
            _label = label;
        }

        public string GetLabel()
        {
            return _label;
        }
    }
}
