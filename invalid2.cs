using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_1_1
{
    public partial class invalid2 : Form
    {
        public invalid2()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LogIn l = new LogIn();
            l.Show();
            this.Hide();
        }

        private void invalid2_Load(object sender, EventArgs e)
        {

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                pictureBox1_Click(this, new EventArgs()); return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
