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
            int heightstep = (int)(numericHeightstep.Value);
            int iterations = (int)(numericIterations.Value);
            int height = pictureBox.Height;
            int width = pictureBox.Width;

            landscapeGenerator.ResetPointList(width, height);

            landscapeGenerator.CalculateLandscape(iterations, height);

            List<Point> points = new List<Point>();
        }
    }
}
