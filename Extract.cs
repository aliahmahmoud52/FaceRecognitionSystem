using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using Finisar.SQLite;
namespace Project_1_1
{
    
    public partial class Extract : Form
    {
        SQLiteConnection sqlite_conn1;
        SQLiteCommand sqlite_cmd1;
        SQLiteDataReader sqlite_datareader1;
        int id1;
        public Extract()
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            First_form Lout = new First_form();
            Lout.Show();
            this.Hide();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                String s = comboBox1.Text;
                StringBuilder w = new StringBuilder();
                sqlite_conn1 = new SQLiteConnection("Data Source=" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/Project.db ; Version =3;");
                sqlite_conn1.Open();
                sqlite_cmd1 = sqlite_conn1.CreateCommand();
                sqlite_cmd1.CommandText = "select student_id , date from attendance where subj_id = '" + s + "';";
                sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                while (sqlite_datareader1.Read())
                {
                    String q = sqlite_datareader1.GetString(0);
                    String q1 = sqlite_datareader1.GetString(1);
                    String m = q + "," + q1;
                    w.AppendLine(m);

                }
                String path = "C:/Users/Alia Mahmoud/Desktop/" + s + ".csv";
                File.AppendAllText(path, w.ToString());
                //MessageBox.Show("attendanc file created in desktop , check it ");
                File_Path fp1 = new File_Path();
                fp1.setId(id1);
                fp1.Show();
                this.Hide();
            }
            catch (Exception ex) { }
            }

        public void setId(int id)
        {
            id1 = id;
          //  Console.WriteLine(""+id1);
        }

        private void Extract_Load(object sender, EventArgs e)
        {
            try
            {
                sqlite_conn1 = new SQLiteConnection("Data Source=" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/Project.db ; Version =3;");
                sqlite_conn1.Open();
                sqlite_cmd1 = sqlite_conn1.CreateCommand();
                sqlite_cmd1.CommandText = "select subject_id from doctorSubjects where doctor_id = " + id1 + ";";
                sqlite_datareader1 = sqlite_cmd1.ExecuteReader();
                while (sqlite_datareader1.Read())
                {
                    String s = sqlite_datareader1.GetString(0);
                    Console.WriteLine(s);
                    comboBox1.Items.Add(s);

                }
                sqlite_conn1.Close();
                sqlite_datareader1.Close();
            }
            catch (Exception ex) { }
        }
    }
}
