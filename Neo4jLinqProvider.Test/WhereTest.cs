using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4jLinqProvider.ExpressionVisitors;
using System.Linq.Expressions;
using Translations.Data.NodeDefinitions;
using System.Linq;

namespace Neo4jLinqProvider.Test
{
    [TestClass]
    public class WhereTest
    {
        private WhereLambdaExpressionEvaluator _evaluator;
        private Arguments _arguments;

        [TestInitialize]
        public void Initialize()
        {
            _arguments = new Arguments();
            _evaluator = new WhereLambdaExpressionEvaluator(_arguments);
        }
        
        [TestMethod]
        public void Where_Equality_Supported()
        {
            var where = GetWhere(x => x.Name == "John");

            Assert.AreEqual("n0.name = {P0}", where);
            Assert.AreEqual(_arguments["P0"], "John");
        }
        
        [TestMethod]
        public void Where_Equality_And_Equality_Supported()
        {
            var where = GetWhere(x => x.Name == "John" && x.Age == 10);

            Assert.AreEqual("n0.name = {P0} AND n0.age == {P1}", where);
            Assert.AreEqual(_arguments["P0"], "John");
            Assert.AreEqual(_arguments["P1"], "10");
        }

        [TestMethod]
        public void Where_Equality_WithVariable_Supported()
        {
            var name = "John";
            var where = GetWhere(x => x.Name == name);

            Assert.AreEqual("n0.name = {P0}", where);
            Assert.AreEqual(_arguments["P0"], "John");
        }

        [TestMethod]
        public void Where_Inquality_Supported()
        {
            var where = GetWhere(x => x.Name != "John");

            Assert.AreEqual("n0.name <> {P0}", where);
            Assert.AreEqual(_arguments["P0"], "John");
        }
        
        [TestMethod]
        public void Where_LessThan_Supported()
        {
            var where = GetWhere(x => x.Age < 10);

            Assert.AreEqual("n0.age < {P0}", where);
            Assert.AreEqual(_arguments["P0"], "10");
        }

        [TestMethod]
        public void Where_LessThanOrEqual_Supported()
        {
            var where = GetWhere(x => x.Age <= 10);

            Assert.AreEqual("n0.age <= {P0}", where);
            Assert.AreEqual(_arguments["P0"], "10");
        }

        [TestMethod]
        public void Where_GreaterThan_Supported()
        {
            var where = GetWhere(x => x.Age > 10);

            Assert.AreEqual("n0.age > {P0}", where);
            Assert.AreEqual(_arguments["P0"], "10");
        }

        [TestMethod]
        public void Where_GreaterThanOrEqual_Supported()
        {
            var where = GetWhere(x => x.Age >= 10);

            Assert.AreEqual("n0.age >= {P0}", where);
            Assert.AreEqual(_arguments["P0"], "10");
        }

        [TestMethod]
        public void Where_BooleanProperty_Supported()
        {
            var where = GetWhere(x => x.IsForeign);

            Assert.AreEqual("n0.isForeign", where);
        }

        [TestMethod]
        public void Where_BooleanProperty_And_BooleanProperty_Supported()
        {
            var where = GetWhere(x => x.IsForeign);

            Assert.AreEqual("n0.isForeign AND n0.isHealthy", where);
        }


        [TestMethod]
        public void Where_ListContains_Supported()
        {
            var names = new [] { "Alice", "Bob" };
            var where = GetWhere(x => names.Contains(x.Name));

            Assert.AreEqual("n0.name IN ({P0}, {P1})", where);
            Assert.AreEqual(_arguments["P0"], "Alice");
            Assert.AreEqual(_arguments["P1"], "Bob");
        }


        [TestMethod]
        public void Where_BooleanConstant_Supported()
        {
            var where = GetWhere(x => true);

            Assert.AreEqual("true", where);
        }

        private string GetWhere(Expression<Predicate<DummyNode>> expression)
        {
            return _evaluator.GetWhere(expression);
        }

        private class DummyNode
        {
            [Property("name")]
            public string Name { get; set; }

            [Property("age")]
            public int Age { get; set; }

            [Property("isForeign")]
            public bool IsForeign { get; set; }

            [Property("isHealthy")]
            public bool IsHealthy { get; set; }
        }
    }
}
