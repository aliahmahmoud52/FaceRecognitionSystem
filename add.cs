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
    public partial class add : Form
    {
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        public add()
        {
            InitializeComponent();
            comboBox1.Items.Add("Doctor");
            comboBox1.Items.Add("Student");
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
            sqlite_conn = new SQLiteConnection("Data Source=" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/Project.db ; Version =3;");
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
            if (comboBox1.Text == "Student")
            {
                sqlite_cmd.CommandText = "insert into studentSubjects values(" + int.Parse(textBox2.Text) + ",'" + textBox1.Text + "');";
            }
            else
            {
                sqlite_cmd.CommandText = "insert into doctorSubjects values(" + int.Parse(textBox2.Text) + ",'" + textBox1.Text + "');";
            }
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Dtrain dt = new Dtrain();
            dt.Show();
            this.Hide();
        }
    }
}
