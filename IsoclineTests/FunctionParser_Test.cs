using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IsoclineShower;
using IsoclineShower.Nodes;

namespace IsoclineTests
{
    [TestClass]
    public class FunctionParser_Test
    {
        [TestMethod]
        public void theParseConstant()
        {
            var node = FunctionParser.Parse("12.5");
            Assert.AreEqual(12.5, node.Value());
        }
    }
}
