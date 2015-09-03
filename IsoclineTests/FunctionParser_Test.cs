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
        private static double delta = Math.Pow(10, -20);

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
            var node = FunctionParser.Parse("12+123.4");
            Assert.AreEqual(12 + 123.4, node.Value(), delta);
        }

        [TestMethod]
        public void ParseSum_withOneVariable()
        {
            var node = FunctionParser.Parse("12+123.4+x");
            var variables = new Dictionary<String, double>() { { "x", 1 } };
            Assert.AreEqual(12 + 123.4 + 1, node.Value(variables), delta);
        }

        [TestMethod]
        public void ParseSum_withTwoVariable()
        {
            var node = FunctionParser.Parse("x+y+50.8");
            var variables = new Dictionary<String, double>() { { "x", 1 }, {"y", 1.5} };
            Assert.AreEqual(1 + 1.5 + 50.8, node.Value(variables), delta);
        }

        [TestMethod]
        public void ParseExspressionWithPriotity1()
        {
            var node1 = FunctionParser.Parse("2*3+50.8");
            var node2 = FunctionParser.Parse("50.8+2*3");

            Assert.AreEqual(2*3 + 50.8, node1.Value(), delta);
            Assert.AreEqual(50.8 + 2 * 3, node2.Value(), delta);
        }

        [TestMethod]
        public void ParseExspressionWithPriotity2()
        {
            var node1 = FunctionParser.Parse("x/y-10");
            var node2 = FunctionParser.Parse("10-x/y");
            var variables = new Dictionary<String, double>() { { "x", 1 }, { "y", 1.5 } };

            Assert.AreEqual(1 / 1.5 - 10, node1.Value(variables), delta);
            Assert.AreEqual(10 - 1 / 1.5, node2.Value(variables), delta);
        }

        [TestMethod]
        public void ParseExspressionWithBrackets()
        {
            var node1 = FunctionParser.Parse("(x+y)*10");
            var node2 = FunctionParser.Parse("10*(x+y)");
            var variables = new Dictionary<String, double>() { { "x", 1 }, { "y", 1.5 } };

            Assert.AreEqual((1 + 1.5) * 10, node1.Value(variables), delta);
            Assert.AreEqual(10 * (1 + 1.5), node2.Value(variables), delta);
        }

        [TestMethod]
        public void ParseExspressionWithUnaryMinus()
        {
            var node1 = FunctionParser.Parse("-(x+4)*10");
            var node2 = FunctionParser.Parse("10*(-x+y)");
            var variables = new Dictionary<String, double>() { { "x", 1 }, { "y", 1.5 } };

            Assert.AreEqual(-(1 + 4) * 10, node1.Value(variables), delta);
            Assert.AreEqual(10 * (-1 + 1.5), node2.Value(variables), delta);
        }
        

        [TestMethod]
        public void ParseExspressionWithThrigonometryFuncs()
        {
            var operations = new List<string>() { "sin", "cos", "tg", "ctg" ,"asin","acos" ,"atg" ,"actg"};
            var variables = new Dictionary<String, double>() { { "x", 0.75 } };
            var values = new Dictionary<String, double>()
            {
                {"sin", Math.Sin(0.75)},
                {"cos", Math.Cos(0.75)},
                {"tg", Math.Tan(0.75)},
                {"ctg", 1 / Math.Tan(0.75)},
                {"asin", Math.Asin(0.75)},
                {"acos", Math.Acos(0.75)},
                {"atg", Math.Atan(0.75)},
                {"actg", -Math.Atan(0.75) + Math.PI / 2},
            };

            foreach (var operation in operations)
            {
                var node = FunctionParser.Parse(operation + "(x)");

                Assert.AreEqual(values[operation], node.Value(variables), delta, "Func: " + operation);
            }
        }

        [TestMethod]
        public void ParseExspressionWithDegree()
        {
            var node = FunctionParser.Parse("x^y");
            var variables = new Dictionary<String, double>() { { "x", 2 }, { "y", 1.5 } };

            Assert.AreEqual(Math.Pow(2 , 1.5), node.Value(variables), delta);
        }

        [TestMethod]
        public void VeryDifficultFunc()
        {
            var node = FunctionParser.Parse("2+sin(x^(tg(y))+20.123)-(-atg(-1000*x)*log((x+y), 2))");
            var variables = new Dictionary<String, double>() { { "x", 2 }, { "y", 1.5 } };

            Assert.AreEqual(2 + Math.Sin(Math.Pow(2, Math.Tan(1.5)) + 20.123) - (-Math.Atan(-1000*2)*Math.Log(3.5,2)), node.Value(variables), delta);
        }

        [TestMethod]
        public void ParseExspressionWithLogarifm()
        {
            var node = FunctionParser.Parse("log((x+y), 2)");
            var variables = new Dictionary<String, double>() { { "x", 3 }, { "y", 5 } };

            Assert.AreEqual(Math.Log((3+5), 2), node.Value(variables), delta);
        }

        [TestMethod]
        public void VeryDifficultFuncWithLambda()
        {
            var f = FunctionParser.CreateFXY("2+sin(x^(tg(y))+20.123)-(-atg(-1000*x)*log((x+y), 2))");
            var value = 2 + Math.Sin(Math.Pow(2, Math.Tan(1.5)) + 20.123) - (-Math.Atan(-1000 * 2) * Math.Log(3.5, 2));
            
            Assert.AreEqual(value, f(2, 1.5), delta);
        }

        [TestMethod]
        public void ParseExspressionWithPiAndE()
        {
            var node = FunctionParser.Parse("sin(pi*x)+e");
            var variables = new Dictionary<String, double>() { { "x", 3 } };

            Assert.AreEqual(Math.Sin(3*Math.PI) + Math.E, node.Value(variables), delta);
        }

        [TestMethod]
        public void ParseExspressionWithPiAndE_2()
        {
            Assert.AreEqual(Math.Sin(Math.PI),FunctionParser.Parse("sin(pi)").Value(), delta);
            Assert.AreEqual(Math.Log(Math.E, Math.E), FunctionParser.Parse("log(e, e)").Value(), delta);
        }
    }
}
