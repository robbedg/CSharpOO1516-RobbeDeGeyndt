using LandscapeGeneratorImplementation;
using LandscapeGeneratorInterface;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace Opgave_3b
{
    public partial class Form1 : Form
    {

        ILandscapeGenerator landscapeGenerator = new LandscapeGenerator();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {

        }

        private void label1_Click(object sender, System.EventArgs e)
        {

        }

        private void buttonRecalculate_Click(object sender, System.EventArgs e)
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            int heightstep = (int)(numericHeightstep.Value);
            int iterations = (int)(numericIterations.Value);
            int height = pictureBox.Height;
            int width = pictureBox.Width;

            landscapeGenerator.ResetPointList(width, height);

            landscapeGenerator.CalculateLandscape(iterations, heightstep);

            List<Point> points = landscapeGenerator.PointList;

            Graphics G = Graphics.FromImage(pictureBox.Image);

            using (var p = new Pen(Color.Blue, 4))
            {
                for (int x = 0; x < points.Count - 1; x++)
                {
                    G.DrawLine(p, points[x], points[x + 1]);
                }
            }
            pictureBox.Refresh();
        }

        private void numericIterations_ValueChanged(object sender, System.EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, System.EventArgs e)
        {
            string name = textNaam.Text;
            landscapeGenerator.SaveLandscape(name);
        }

        private void buttonLoad_Click(object sender, System.EventArgs e)
        {
            string name = textNaam.Text;
            landscapeGenerator.LoadLandscape(name);

            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);

            List<Point> points = landscapeGenerator.PointList;

            Graphics G = Graphics.FromImage(pictureBox.Image);

            using (var p = new Pen(Color.Blue, 4))
            {
                for (int x = 0; x < points.Count - 1; x++)
                {
                    G.DrawLine(p, points[x], points[x + 1]);
                }
            }
            pictureBox.Refresh();
        }
    }
}
