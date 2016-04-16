using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoclineShower
{
    public class FunctionExstensions
    {
        private static double delta = Math.Pow(10, -5);

        public static Func<double, double> df_dx(Func<double, double> f)
        {
            return (x) => (f(x + delta/2) - f(x - delta/2))/delta;
        }

        public static Func<double, double, Tuple<double, double>> Grad(Func<double, double, double> f)
        {

            return (x, y) => Tuple.Create((f((x + delta / 2), y) - f((x - delta / 2), y)) / delta, (f(x, (y + delta / 2)) - f(x, (y - delta / 2))) / delta);
        }

        public static Tuple<double, double> FindPointOfIsoline(Func<double, double, double> f, double k)
        {
            double x = 0.0, y = 0.0;
            var gradF = Grad(f);
            var step = 1.0;
            var point = Tuple.Create(x, y);
            while (Math.Abs(f(x,y) - k) > delta)
            {
                if (Math.Abs(f(x, y) - k) > 1)
                {
                    step = Math.Abs(step - 0.01);
                }
                else
                {
                    step = Math.Abs(f(x, y) - k)*delta;
                }
                point = Norm(gradF(x, y));
                if ((point.Item1==0) && (point.Item2==0))
                {
                    x = x + 1;
                    continue;
                }
                if (f(x, y) < k)
                {
                    x = x + step * point.Item1;
                    y = y + step * point.Item2;
                }
                else
                {
                    x = x - step * point.Item1;
                    y = y - step * point.Item2;
                }
            }
            return Tuple.Create(x, y);
        }

        private static Tuple<double, double> Norm(Tuple<double, double> p)
        {
            var length = Math.Sqrt(p.Item1 * p.Item1 + p.Item2 * p.Item2);
            return Tuple.Create(p.Item1 / length, p.Item2 / length);
        }


        public static List<Tuple<double, double>> FindIsoline(Func<double, double, double> f, double k)
        {
            var point = FindPointOfIsoline(f, k);
            var listOfPoints = new List<Tuple<double, double>>();
            listOfPoints.Add(point);
            var gradF = Grad(f);
            //TODO
            return listOfPoints;
        }
    }
}
