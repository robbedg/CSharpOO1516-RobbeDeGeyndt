using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opgave01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            trackBar1.Visible = false;
            pictureBox1.Image = new Bitmap(Image.FromFile("TestIMG.jpg"));
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //Button Red
        private void button1_Click(object sender, EventArgs e)
        {
            Color color = Color.Black;

            byte g = color.G;
            byte b = color.B;

            Bitmap bmp = new Bitmap(Image.FromFile("TestIMG.jpg"));
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color gotColor = bmp.GetPixel(x, y);
                    gotColor = Color.FromArgb(gotColor.R, g, b);
                    bmp.SetPixel(x, y, gotColor);
                }
            }
            pictureBox2.Image = bmp;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(Image.FromFile("TestIMG.jpg"));
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        //Buton Green
        private void button1_Click_1(object sender, EventArgs e)
        {
            Color color = Color.Black;

            byte r = color.R;
            byte b = color.B;

            Bitmap bmp = new Bitmap(Image.FromFile("TestIMG.jpg"));
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color gotColor = bmp.GetPixel(x, y);
                    gotColor = Color.FromArgb(r, gotColor.G, b);
                    bmp.SetPixel(x, y, gotColor);
                }
            }
            pictureBox2.Image = bmp;
        }

        //Button Blauw
        private void buttonBlauw_Click(object sender, EventArgs e)
        {
            Color color = Color.Black;

            byte r = color.R;
            byte g = color.G;

            Bitmap bmp = new Bitmap(Image.FromFile("TestIMG.jpg"));
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color gotColor = bmp.GetPixel(x, y);
                    gotColor = Color.FromArgb(r, g, gotColor.B);
                    bmp.SetPixel(x, y, gotColor);
                }
            }
            pictureBox2.Image = bmp;
        }

        //Scrollbar
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int bar = -(trackBar1.Value) + 8;
            Bitmap bmp = new Bitmap(Image.FromFile("TestIMG.jpg"));
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color gotColor = bmp.GetPixel(x, y);
                    gotColor = Color.FromArgb(gotColor.R >> bar, gotColor.G >> bar, gotColor.B >> bar);
                    bmp.SetPixel(x, y, gotColor);
                }
            }
            pictureBox2.Image = bmp;
        }

        //Button Resolution.
        private void buttonResolution_Click(object sender, EventArgs e)
        {
            trackBar1.Visible = true;
            
        }
    }
}
