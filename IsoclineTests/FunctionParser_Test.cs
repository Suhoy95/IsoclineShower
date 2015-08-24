using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IsoclineShower;
using IsoclineShower.Nodes;

namespace IsoclineTests
{
    [TestClass]
    public class FunctionParser_Test
    {
        private static double delta = Math.Pow(10, -8);

        [TestMethod]
        public void ParseConstant_integralNumbers()
        {
            var node = FunctionParser.Parse("123");
            Assert.AreEqual(123, node.Value(), delta);
        }

        [TestMethod]
        public void ParseConstant_withDot()
        {
            var node = FunctionParser.Parse("12.5");
            Assert.AreEqual(12.5, node.Value(), delta);
        }

        [TestMethod]
        public void ParseVariable()
        {
            var node = FunctionParser.Parse("x");
            var variables = new Dictionary<String, double>() {{"x", 12.5}};
            Assert.AreEqual(12.5, node.Value(variables));
        }

        [TestMethod]
        public void ParseSum()
        {
            var node = FunctionParser.Parse("12 + 123.4");
            Assert.AreEqual(12 + 123.4, node.Value(), delta);
        }

        [TestMethod]
        public void ParseSum_withOneVariable()
        {
            var node = FunctionParser.Parse("12 + 123.4 + x");
            var variables = new Dictionary<String, double>() { { "x", 1 } };
            Assert.AreEqual(12 + 123.4 + 1, node.Value(variables), delta);
        }

        [TestMethod]
        public void ParseSum_withTwoVariable()
        {
            var node = FunctionParser.Parse("x + y + 50.8");
            var variables = new Dictionary<String, double>() { { "x", 1 }, {"y", 1.5} };
            Assert.AreEqual(1 + 1.5 + 50.8, node.Value(variables), delta);
        }
    }
}
