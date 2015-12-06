using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicInterface;
using LogicImplementation;

namespace FilterGUI
{
    public partial class Form1 : Form
    {
        IImageFilter filter = new ImageFilter();
        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.Multiselect = false;
            string imagePath;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = dialog.FileName;
                filter.Load(imagePath);
                pictureBox1.Image = filter.FilteredImage;
            }

            
        }

        private void selectFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Enum.GetNames(typeof(Filter)).Contains(selectFilter.SelectedItem.ToString()) && pictureBox1.Image != null)
            {
                filter.Progress += updateBar;
                filter.ApplyFilter((Filter)Enum.Parse(typeof(Filter), selectFilter.SelectedItem.ToString()));
                pictureBox1.Image = filter.FilteredImage;
                filter.Progress -= updateBar;
            }
        }

        private void updateBar(int x)
        {
            progressBar1.Value = x;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
