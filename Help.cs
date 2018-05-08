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
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            How_to_use htu = new How_to_use();
            htu.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            aboutUs au = new aboutUs();
            au.Show();
            this.Hide();
        }

        private void Help_Load(object sender, EventArgs e)
        {

        }
    }
}
