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
    public partial class info_train : Form
    {
        public info_train()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            info info = new info();
            info.Show();
            this.Hide();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Dtrain train = new Dtrain();
            train.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            First_form Lout = new First_form();
            Lout.Show();
            this.Hide();
        }

        private void info_train_Load(object sender, EventArgs e)
        {

        }
    }
}
