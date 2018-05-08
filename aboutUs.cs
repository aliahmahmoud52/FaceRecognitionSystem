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
    public partial class aboutUs : Form
    {
        public aboutUs()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            First_form htu = new First_form();
            htu.Show();
            this.Hide();
        }

        private void aboutUs_Load(object sender, EventArgs e)
        {

        }
    }
}
