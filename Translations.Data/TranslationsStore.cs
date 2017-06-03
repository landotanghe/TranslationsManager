using System.Collections.Generic;
using Neo4j.Driver.V1;
using System;
using Translator.Shared;
using System.Linq;
using System.Linq.Expressions;
using Translations.Data.Nodes;
using Translations.Data.NodeDefinitions;
using System.Reflection;

namespace Translations.Data
{
    public class TranslationsStore
    {
        private Language _baseLanguage;
        public TranslationsStore(Language baseLanguage)
        {
            _baseLanguage = baseLanguage;
        }

        public void AddTranslation(string word, string translation, Language language)
        {
            if (!FindWord(word).Any())
            {
                CreateWord(word);
            }
            if(!FindTranslation(translation, language).Any())
            {
                CreateTranslation(translation, language);
            }
            
             Link(word, translation, language);
        }

        private class CypherMatchBuilder
        {
            private string _variableName;
            private List<string> _labels;
            private Dictionary<string, string> _propertyArguments;

            public CypherMatchBuilder(string variableName)
            {
                _labels = new List<string>();
                _variableName = variableName;
                _propertyArguments = new Dictionary<string, string>();
            }

            public CypherMatchBuilder Match(Type type)
            {
                var labels = type.GetCustomAttributes(true).OfType<NodeAttribute>().Select(n => n.GetLabel());
                _labels.AddRange(labels);
                _labels = _labels.Distinct().ToList();

                return this;
            }

            public CypherMatchBuilder PropertyIs(PropertyInfo property, string argumentName)
            {
                var nodeProperty = (PropertyAttribute) property.GetCustomAttribute(typeof(PropertyAttribute));
                var propertyName = nodeProperty.GetName();
                _propertyArguments.Add(propertyName, argumentName);

                return this;
            }

            public string ToString()
            {
                var labelsFilter = String.Join("", _labels.Select(label => $":{label}"));
                var propertiesFilter = _propertyArguments.Any() ? "{" + String.Join(",", _propertyArguments.Select(pv => pv.Key + ":{" + pv.Value +"}" ))+ "}" : String.Empty;

                return "MATCH (" + _variableName + labelsFilter + propertiesFilter + ")";
            }

        }

        public Word GetWord(string word)
        {
            var variableName = "myWord";
            var argument = Arg("wordValue", word);

            var matchBuilder = new CypherMatchBuilder(variableName);

            var match = matchBuilder
                .Match(typeof(Word))
                .PropertyIs(typeof(Word).GetProperty("Name"), argument.Name)
                .ToString();

            var query = $"{match} return {variableName}.name, {variableName}.language";

            var result = ExecueQuery(query, new []{ argument });
            var wordRow = result.First();
            return new Word
            {
                Name = (string) wordRow[0],
                Language = (string) wordRow[1]
            };
        }

        public void Categorise(string category, params string[] words)
        {
            if (!FindCategory(category).Any())
            {
                CreateCategory(category);
            }
            Link(category, words);
        }

        public IEnumerable<string> GetWordsInCategory(string name)
        {
            var result = ExecueQuery("MATCH (word:Word)-[:IsPartOf]->(category:Category { name: {name}})"
                + "RETURN word.name",
                Arg("name", name));

            return result.Select(r => r[0].ToString()).ToList();
        }

        private void Link(string category, string[] words)
        {
            var matches = "MATCH (category:Category {name: {category}})";
            var creates = "";
            var arguments = new List<Argument>();

            arguments.Add(Arg("category", category));
            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i];
                var argument = $"word{i}";
                matches += "MATCH(" + argument + ":Word { name: {" + argument + "} })" + "   ";
                creates += "CREATE UNIQUE(" + argument + ")-[:IsPartOf]->(category)" + "   ";
                arguments.Add(Arg(argument, word));
            }
            ExecueQuery(matches + " " + creates, arguments.ToArray());
        }

        private void Link(string word, string translation, Language language)
        {
            ExecueQuery("MATCH (word:Word {name:{word}})" +
                "MATCH (translation:Translation {name:{translation}, language: {language} })" + 
                "CREATE UNIQUE (translation)-[:TranslatesTo]->(word)",
                Arg("word", word), Arg("translation", translation),
                Arg("language", language.GetName()));
        }

        private IEnumerable<string> TranslationsOf(string name, Language language)
        {
            var lang = language.GetName();
            var result = ExecueQuery("MATCH (translation:Translation {language: {language} })-[:TranslatesTo]->(word:Word {name: {name} })" +
                               "RETURN translation.name",
                               Arg("name", name), Arg("language", lang));

            var translations = result.Select(r => r[0].ToString()).ToList();
            return translations;
        }

        private IStatementResult FindTranslation(string word, Language language)
        {
            return FindByName(word, ":Translation", language);
        }

        private IStatementResult FindWord(string word)
        {
            return FindByName(word, ":Word", _baseLanguage);
        }

        private IStatementResult FindCategory(string word)
        {
            return FindByName(word, ":Category", _baseLanguage);
        }

        private IStatementResult FindByName(string name, string label, Language language)
        {
            return ExecueQuery($"MATCH (word{label}) "+ "WHERE word.name = {name} and word.language = {language}" +
                               "RETURN word",
                               Arg("name", name), Arg("language", language.GetName()));
        }

        private void CreateTranslation(string name, Language language)
        {
            ExecueQuery("CREATE (word:Translation {name: {name}, language: {language}})",
                Arg("name", name), Arg("language", language.GetName()));
        }

        private void CreateWord(string name)
        {
            ExecueQuery("CREATE (word:Word  {name: {name}, language: {language} })",
                Arg("name", name), Arg("language", _baseLanguage.GetName()));
        }

        private void CreateCategory(string name)
        {
            ExecueQuery("CREATE (word:Category {name: {name}, language: {language} })",
                Arg("name", name), Arg("language", _baseLanguage.GetName()));
        }

        private Argument Arg(string name, object value)
        {
            return new Argument
            {
                Name = name,
                Value = value
            };
        }

        private class Argument
        {
            public string Name;
            public object Value;
        }

        
        private IStatementResult ExecueQuery(string query, params Argument[] arguments)
        {
            var dictionary = arguments.ToDictionary(arg => arg.Name, arg => arg.Value);
            return ExecueQuery(query, dictionary);
        }

        private IStatementResult ExecueQuery(string query, Dictionary<string, object> arguments)
        {
            using (var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "test")))
            using (var session = driver.Session())
            {
                var result = session.Run(query, arguments);
                return result;
            }
        }        
    }
}
