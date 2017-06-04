using System.Collections.Generic;

namespace Translations.Data.CypherBuilders
{
    class CypherArgumentBuilder
    {
        private int index;
        private List<Argument> Arguments = new List<Argument>();

        public string GetNextArgumentName()
        {
            return $"arg{index++}";
        }

        public void SetValue(string argumentName, string value)
        {
            Arguments.Add(new Argument
            {
                Name = argumentName,
                Value = value
            });
        }

        public Argument[] GetArguments()
        {
            return Arguments.ToArray();
        }
    }
}
