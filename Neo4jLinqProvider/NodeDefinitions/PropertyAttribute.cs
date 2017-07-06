using System;

namespace Translations.Data.NodeDefinitions
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class PropertyAttribute : Attribute
    {
        private string _name;

        public PropertyAttribute(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }
    }
}
