using Microsoft.VisualStudio.TestTools.UnitTesting;
using Translator.Shared;
using Translations.Data.Nodes;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Translations.Data.Test
{
    [TestClass]
    public class TestMatchers
    {
        //improve tests by not using store, but using the querybuilders instead to add data
        //note that connection settings are configured twice because of this
        private TranslationsStore store = new TranslationsStore(Language.Dutch, "bolt://localhost:7687", "neo4j", "test");

        [TestInitialize]
        public void Init()
        {
            ConfigurationManager.AppSettings.Set("database.url", "bolt://localhost:7687");
            ConfigurationManager.AppSettings.Set("database.username", "neo4j");
            ConfigurationManager.AppSettings.Set("database.password", "test");
        }

        [TestCleanup]
        public void Cleanup()
        {
            // needs improvement, deletes all nodes and relationships, but 'Word' is passed in
            // to comply with generic
            CypherBuilders.CypherQueryBuilder<Word>.DeleteAll();
        }

        [TestMethod]
        public void Match_EqualsOperator_ByPassingInVariable_ValueExists_ReturnsThatValue()
        {

            AddWord("koe");
            AddWord("schaap");
            AddWord("kip");
            AddWord("aap");

            string wordToFind = "koe";
            var query = new CypherBuilders.CypherQueryBuilder<Word>()
                .Match(word => word.Name == wordToFind);
            var first = query.FirstOrDefault();

            Assert.AreEqual("koe", first.Name);
            Assert.AreEqual("du", first.Language);
        }
        
        [TestMethod]
        public void Match_EqualsOperator_NotFound_ReturnsDefaultValue()
        {
            AddWord("schaap");

            string wordToFind = "koe";
            var query = new CypherBuilders.CypherQueryBuilder<Word>()
                .Match(word => word.Name == wordToFind);
            var first = query.FirstOrDefault();

            Assert.AreEqual(null, first);
        }

        [TestMethod]
        public void Match_ListContains_ReturnsAllValuesFound()
        {
            AddWord("schaap");
            AddWord("koe");
            AddWord("kip");

            List<string> wordToFind = new List<string> { "koe", "kip" };
            var query = new CypherBuilders.CypherQueryBuilder<Word>()
                .Match(word => wordToFind.Contains(word.Name));

            var matches = query.ToList();
            Assert.IsTrue(matches.Any(w => w.Name == "koe"));
            Assert.IsTrue(matches.Any(w => w.Name == "kip"));
        }


        [TestMethod]
        public void Match_EqualsOperator_ByPassingInLiteralInline_ValueExists_ReturnsThatValue()
        {
            AddWord("koe");
            
            var query = new CypherBuilders.CypherQueryBuilder<Word>()
                .Match(word => word.Name == "koe");
            var first = query.FirstOrDefault();

            Assert.AreEqual("koe", first.Name);
            Assert.AreEqual("du", first.Language);
        }

        //improve tests by not using store, but using the querybuilders instead to add data
        //note that connection settings are configured twice because of this
        private void AddWord(string word)
        {
            store.AddTranslation(word, word, Language.English);
        }
    }
}
