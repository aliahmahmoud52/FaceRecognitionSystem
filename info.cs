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
    public partial class info : Form
    {

        List<string> subj = new List<string>();

        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;

        public info()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                pictureBox2_Click(this, new EventArgs()); return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void info_Load(object sender, EventArgs e)
        {
           

            comboBox3.Items.Add("Saturday");
            comboBox3.Items.Add("Sunday");
            comboBox3.Items.Add("Monday");
            comboBox3.Items.Add("Tuesday");
            comboBox3.Items.Add("Wednesday");
            comboBox3.Items.Add("Thursday");

            comboBox4.Items.Add("7");
            comboBox4.Items.Add("8");
            comboBox4.Items.Add("9");
            comboBox4.Items.Add("10");
            comboBox4.Items.Add("11");
            comboBox4.Items.Add("12");
            comboBox4.Items.Add("13");
            comboBox4.Items.Add("14");
            comboBox4.Items.Add("15");
            comboBox4.Items.Add("16");
            comboBox4.Items.Add("17");
            comboBox4.Items.Add("18");



            comboBox5.Items.Add("9");
            comboBox5.Items.Add("10");
            comboBox5.Items.Add("11");
            comboBox5.Items.Add("12");
            comboBox5.Items.Add("13");
            comboBox5.Items.Add("14");
            comboBox5.Items.Add("15");
            comboBox5.Items.Add("16");
            comboBox5.Items.Add("17");
            comboBox5.Items.Add("18");
            comboBox5.Items.Add("19");
            comboBox5.Items.Add("20");
            comboBox5.Items.Add("21");
            comboBox5.Items.Add("22");
            comboBox5.Items.Add("23");

            comboBox2.Items.Add("Lab 1");
            comboBox2.Items.Add("Lab 2");
            comboBox2.Items.Add("Lab 3");
            comboBox2.Items.Add("10");
            comboBox2.Items.Add("13");
            comboBox2.Items.Add("14");
            comboBox2.Items.Add("15");
            comboBox2.Items.Add("16");

            try
            {
                sqlite_conn = new SQLiteConnection("Data Source="+Environment.GetEnvironmentVariable("appdata")+"/Project_1_1/Project.db ; Version =3;");
                comboBox2.Items.Add("17");
                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();
                //create command
                sqlite_cmd.CommandText = "select subj_id from classRoomT ;";
                //datareader to read from db
                sqlite_datareader = sqlite_cmd.ExecuteReader();

                while (sqlite_datareader.Read())
                {
                    subj.Add(sqlite_datareader.GetString(0));
                }

                sqlite_cmd.ExecuteNonQuery();
                for (int i = 0; i < subj.Count; i++)
                {
                    comboBox1.Items.Add(subj[i]);
                    Console.WriteLine(subj[i]);
                }
                sqlite_conn.Close();
                sqlite_datareader.Close();


            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            info_train info = new info_train();
            info.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                sqlite_conn = new SQLiteConnection("Data Source=" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/Project.db ; Version =3;");
                sqlite_conn.Open();

                String subj = comboBox1.Text;
                String place = comboBox2.Text;
                String day = comboBox3.Text;
                String from = comboBox4.Text;
                String to = comboBox5.Text;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "insert into classRoomT values ('" + subj + "','" + place + "','" + day + "','" + from + "','" + to + "');";
                sqlite_cmd.ExecuteNonQuery();
                sqlite_conn.Close();
                Submity sub = new Submity();
                sub.Show();
                this.Hide();
            }
            catch (Exception ex) { }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

            comboBox1.Items.Add(textBox1.Text);
            textBox1.Clear();


        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
            
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}