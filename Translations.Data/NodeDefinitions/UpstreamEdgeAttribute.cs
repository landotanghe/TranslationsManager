using System;

namespace Translations.Data.NodeDefinitions
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class UpstreamEdgeAttribute : Attribute
    {
        private string _label;

        public UpstreamEdgeAttribute(string label)
        {
            _label = label;
        }
    }
}
