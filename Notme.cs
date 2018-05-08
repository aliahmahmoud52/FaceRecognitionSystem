using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Finisar.SQLite;
namespace Project_1_1
{
    public partial class Notme : Form
    {
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;
        public Notme()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                sqlite_conn = new SQLiteConnection("Data Source=" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/Project.db ; Version =3;");
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_conn.Open();
                sqlite_cmd.CommandText = "select password from doctor where doc_id =" + textBox3.Text + ";";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                String s = "";
                while (sqlite_datareader.Read())
                {
                    s = sqlite_datareader.GetString(0);

                }
                if (s == textBox2.Text)
                {
                    Extract ex = new Extract();
                    ex.setId(int.Parse(textBox3.Text));
                    ex.Show();
                    this.Hide();
                }
                else
                {
                    Invalid ip = new Invalid();
                    ip.Show();
                    this.Hide();
                }
                sqlite_cmd.ExecuteNonQuery();
                sqlite_conn.Close();
                sqlite_datareader.Close();
            }
            catch (Exception ex) { }
        }

        private void Notme_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                pictureBox2_Click(this, new EventArgs()); return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LogIn lg = new LogIn();
            lg.Show();
            this.Hide();
        }
    }
}
