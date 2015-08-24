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
        public void ParseConstant()
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
    }
}
