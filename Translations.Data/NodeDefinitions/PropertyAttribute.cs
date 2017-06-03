using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
