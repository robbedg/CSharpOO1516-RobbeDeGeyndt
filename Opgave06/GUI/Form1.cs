using GlobalTools;
using LogicImplementation;
using LogicInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    
    public partial class Form1 : Form
    {
        IMD5CollisionCalculator mcc = new MD5CollisionCalculator();
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            mcc.CollisionFound += CollisionHandler;
            String hash = MD5Calculator.GetHash(textBox1.Text);
            mcc.StartCalculatingMD5Collision(hash, (int)UpDown.Value);
            //(int)UpDown.Value


        }

        private void CollisionHandler(string woord)
        {
            mcc.Abort();
            //Form1.Sh
            this.textBoxOut.Text = "test";
            this.textBoxOut.Refresh();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
