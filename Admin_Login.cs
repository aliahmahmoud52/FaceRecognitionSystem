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
    public partial class Admin_Login : Form
    {
        public Admin_Login()
        {
            InitializeComponent();
        }

        private void Admin_Login_Load(object sender, EventArgs e)
        {
            textBox1.PasswordChar= '*';
        }
      
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                pictureBox1_Click(this, new EventArgs()); return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            TextBox t = (TextBox)textBox1;
            string s = t.Text;
            if (s == "Admin")
            {
                info_train info_Train = new info_train();
                info_Train.Show();
                this.Hide();
            }
            else {
                Invalid_Password invalid_pass = new Invalid_Password();
                invalid_pass.Show();
                this.Hide();

            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            First_form ft = new First_form();
            ft.Show();
            this.Hide();
        }
    }
}
