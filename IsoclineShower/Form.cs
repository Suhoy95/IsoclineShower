using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace IsoclineShower
{
    class MyForm : Form
    {
        private static Font font = new Font("Times New Roman", 20, FontStyle.Bold);
        private static int controlsHeight = 40;

        private Label label_fxy;
        private TextBox box_fxy;

        private Label l_min;
        private TextBox b_min;

        private Label l_max;
        private TextBox b_max;
                
        private Label l_x;
        private TextBox b_x;

        private Label l_y;
        private TextBox b_y;

        private PictureBox picture;
        private Button button;

        public MyForm()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Решение задачи Коши";
        
            label_fxy = new Label{ Text = "Введите функцию y' = [f(x,y)]", Dock = DockStyle.Fill, Font = font, Height = controlsHeight };
            box_fxy = new TextBox{ Dock = DockStyle.Fill, Font = font };

            l_min = new Label{ Text = "Min:", Dock = DockStyle.Fill, Font = font, Height = controlsHeight };
            b_min = new TextBox{ Dock = DockStyle.Fill, Font = font };
            l_max = new Label{ Text = "Max:", Dock = DockStyle.Fill, Font = font, Height = controlsHeight };
            b_max = new TextBox{ Dock = DockStyle.Fill, Font = font };
        
            l_x = new Label{ Text = "X0", Dock = DockStyle.Fill, Font = font, Height = controlsHeight };
            b_x = new TextBox{ Dock = DockStyle.Fill, Font = font };
            l_y = new Label{ Text = "Y0:", Dock = DockStyle.Fill, Font = font, Height = controlsHeight };
            b_y = new TextBox{ Dock = DockStyle.Fill, Font = font };
        
            button = new Button {Text = "Нарисовать", Dock = DockStyle.Fill, Font = font, Height = controlsHeight + 10 };
            picture = new PictureBox(){ Dock = DockStyle.Fill };
            
            var table = new TableLayoutPanel();
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, label_fxy.Height));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, box_fxy.Height));


            table.RowStyles.Add(new RowStyle(SizeType.Absolute, l_min.Height));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, b_min.Height));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, l_max.Height));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, b_max.Height));

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, l_x.Height));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, b_x.Height));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, l_y.Height));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, b_y.Height));

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, button.Height));
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            table.Controls.Add(label_fxy, 0, 0);
            table.Controls.Add(box_fxy, 0, 1);

            table.Controls.Add(l_min, 0, 2);
            table.Controls.Add(b_min, 0, 3);
            table.Controls.Add(l_max, 0, 4);
            table.Controls.Add(b_max, 0, 5);

            table.Controls.Add(l_x, 0, 6);
            table.Controls.Add(b_x, 0, 7);
            table.Controls.Add(l_y, 0, 8);
            table.Controls.Add(b_y, 0, 9);

            table.Controls.Add(button, 0, 10);
            table.Controls.Add(new Panel(), 0, 11);
            table.Controls.Add(picture, 1, 0);
            table.SetRowSpan(picture, 12);
            table.Dock = DockStyle.Fill;
            Controls.Add(table);


            PrintGraphics(null, null);
            this.Resize += PrintGraphics;
            button.Click += PrintGraphics;
        }

        public void PrintGraphics(object sender, EventArgs args)
        {
            try
            {
                var f = FunctionParser.CreateFXY(box_fxy.Text);
                var min = double.Parse(b_min.Text);
                var max = double.Parse(b_max.Text);
                var x0 = double.Parse(b_x.Text);
                var y0 = double.Parse(b_y.Text);
                var gm = new GraphMaster(min, max);
                
                picture.Image = gm.CreateGraph(picture.Width, picture.Height, 
                    (g) => gm.DrawSolution(g, f, x0, y0) );
            }
            catch(Exception e)
            {
              //  Console.WriteLine(e.Message);
            };            
        }
    }
}
