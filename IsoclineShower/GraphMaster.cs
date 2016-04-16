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
        private double min = -10;
        private double max = 10;
        private int width;
        private int height;
        private static Font font = new Font("Times New Roman", 14);

        public GraphMaster(double _min, double _max)
        {
            min = _min;
            max = _max;
        }

        public Image CreateGraph(int w, int h, Action<Graphics> drawing)
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

        private void DrawCoord(Graphics g)
        {
            g.DrawLine(Pens.Black, Transform(0, min), Transform(0, max));
            g.DrawLine(Pens.Black, Transform(min, 0), Transform(max, 0));
            var step = (max - min) / 20;
            for (var i = min; i < max; i += step)
            {
                g.DrawString(String.Format("{0}", Math.Round(i, 2)), font, Brushes.Black, Transform(0, i));
                g.DrawString(String.Format("{0}", Math.Round(i, 2)), font, Brushes.Black, Transform(i, 0));
            }
        }

        private Point Transform(double x, double y)
        {

            return new Point((int)((x + (max - min) / 2) / (max - min) * width),
                             (int)((-y + (max - min) / 2) / (max - min) * height));
        }

        public void DrawFX(Graphics g, Func<double, double> f)
        {
            for(var i = min; i <= max; i+= 0.001)
            {
                try
                {
                    g.DrawLine(Pens.Black, Transform(i - 0.01, f(i - 0.01)), Transform(i, f(i)));
                } catch {};
            }
        }

        public void DrawSolution(Graphics g,  Func<double, double, double> dy, double x0, double y0)
        {
            double x = x0, y = y0, step = 0.001;
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
            x = x0; y = y0;
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
