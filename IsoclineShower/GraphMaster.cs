using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace IsoclineShower
{
    class GraphMaster
    {
        private static double min = -10;
        private static double max = 10;
        private static int width;
        private static int height;
        private static Font font = new Font("Times New Roman", 14);

        public static Image CreateGraph(int w, int h, Action<Graphics> drawing)
        {
            width = w;
            height = h;
            var bmp = new Bitmap(w, h);
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.White, 0, 0, w, h);
                drawing(g);
                DrawCoord(g);
            }
            return bmp;
        }

        private static void DrawCoord(Graphics g)
        {
            g.DrawLine(Pens.Black, Transform(0, min), Transform(0, max));
            g.DrawLine(Pens.Black, Transform(min, 0), Transform(max, 0));
            for (var i = min; i < max; i += 1.0)
            {
                g.DrawString(String.Format("{0}", i), font, Brushes.Black, Transform(0, i));
                g.DrawString(String.Format("{0}", i), font, Brushes.Black, Transform(i, 0));
            }
        }

        private static Point Transform(double x, double y)
        {

            return new Point((int)((x + (max - min) / 2) / (max - min) * width),
                             (int)((-y + (max - min) / 2) / (max - min) * height));
        }

        public static void DrawFX(Graphics g, Func<double, double> f)
        {
            for(var i = min; i <= max; i+= 0.001)
            {
                try
                {
                    g.DrawLine(Pens.Black, Transform(i - 0.01, f(i - 0.01)), Transform(i, f(i)));
                } catch {};
            }
        }

        public static void DrawSolution(Graphics g,  Func<double, double, double> dy)
        {
            double x = 0, y = -2, step = 0.001;
            while(x <= max)
            {
                var k = dy(x, y);
                var nx = x + step;
                var ny = y + step * k;
                try
                {
                    g.DrawLine(Pens.Black, Transform(x, y), Transform(nx, ny));
                }
                catch { };
                x = nx; y = ny;
            }
            x = 0; y = -2;
            while (x >= min)
            {
                var k = dy(x, y);
                var nx = x - step;
                var ny = y - step * k;
                try
                {
                    g.DrawLine(Pens.Black, Transform(x, y), Transform(nx, ny));
                }
                catch { };
                x = nx; y = ny;
            }
        }
        
    }
}
