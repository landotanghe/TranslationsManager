using System;

namespace Translations.Data.NodeDefinitions
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class DownstreamEdgeAttribute : Attribute
    {
        private string _label;

        public DownstreamEdgeAttribute(string label)
        {
            _label = label;
        }
    }
}
