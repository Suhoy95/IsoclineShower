using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IsoclineShower;

namespace IsoclineTests
{
    [TestClass]
    public class FunctionExstensions_Test
    {
        public static double delta = Math.Pow(10, -5);
        public static double min = -10;
        public static double max = 10;
        public static double step = 0.1;

        public void FuncCompare_R1_to_R1(Func<double, double> f_expected, Func<double, double> g_actual)
        {
            for(var x = min; x <= max; x+= step)
                Assert.AreEqual(f_expected(x), g_actual(x), delta);
        }

        public void FuncCompare_R2_to_R1(Func<double, double, Tuple<double, double>> f_expected,
                                         Func<double, double, Tuple<double, double>> g_actual)
        {
            for(var x = min; x <= max; x += step)
                for (var y = min; y <= max; y += step)
                {
                    var expected = f_expected(x, y);
                    var actual = g_actual(x, y);
                    Assert.AreEqual(expected.Item1, actual.Item1, delta);
                    Assert.AreEqual(expected.Item2, actual.Item2, delta);
                }
        }

        [TestMethod]
        public void Test_df_dx()
        {
            Func<double, double> f_actual = FunctionExstensions.df_dx( (double x) => x*x);
            Func<double, double> g_expected = (x) => 2*x;

            FuncCompare_R1_to_R1(g_expected, f_actual);
        }

        [TestMethod]
        public void Test_Grad()
        {
            var f_actual = FunctionExstensions.Grad((x, y) => x*x + y*y);
            Func<double, double, Tuple<double, double>> g_expected = (x, y) => Tuple.Create(2*x, 2*y);
            
            FuncCompare_R2_to_R1(g_expected, f_actual);
        }

        [TestMethod]
        public void Test_FindPoint()
        {
            Func<double, double, double> f = (x, y) => Math.Sin(x + y) ;
            var k = -0.94;

            var point = FunctionExstensions.FindPointOfIsoline(f, k);

            Assert.IsTrue( Math.Abs(f(point.Item1, point.Item2) - k) < delta );
        }

        [TestMethod]
        [Ignore]
        public void Test_Findisoline()
        {
            Func<double, double, double> f = (x, y) => x * x + y * y;
            var k = 5.0;

            var isoline = new List<Tuple<double, double>>();
            for(var fi = 0.0; fi <= 2*Math.PI; fi += Math.Pow(10, -10))
                isoline.Add(Tuple.Create(k * Math.Cos(fi), k * Math.Sin(fi)));

            var isoline_actual = FunctionExstensions.FindIsoline(f, k);

            foreach (var point in isoline_actual)
            {
                // ищем ближайшие точки реальной изолинии
                var possiblePoint = isoline.Where(p =>
                                                    p.Item1 - point.Item1 < delta &&
                                                    p.Item2 - point.Item2 < delta);
 
                // если найдется  хоть одна такая точка, значит мы смогли сделать функцию,
                // находящую точки изолинии 
                Assert.IsTrue(possiblePoint.Any());
            }
        }
    }
}
