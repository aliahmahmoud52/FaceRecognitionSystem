using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.IO;
using Finisar.SQLite;
using System.Globalization;

namespace Project_1_1
{
    public partial class First_form : Form
    {
        DateTime datetime = DateTime.Now;
        private VideoCapture _capture;
        private CascadeClassifier _cascadeClassifier;
        Image<Gray, byte> TrainedFace = null, unknownFace = null;
        private CascadeClassifier trainCascadeClassifier;
        List<Image<Gray, byte>> trainingImagesHolder = new List<Image<Gray, byte>>();
        List<string> labelsHolder = new List<string>();
        int[] integerLablesHolder;
        int numberOfLables, CountTrainedFaces;
        MEigenRecognizer myStudentRecognizer , myDoctorRecognizer;
        SQLiteConnection sqlite_conn, sqlite_conn1, sqlite_conn2;
        SQLiteCommand sqlite_cmd, sqlite_cmd1, sqlite_cmd2;
        SQLiteDataReader sqlite_datareader, sqlite_datareader1;
        List<string> image = new List<string>();
        List<string> Labels = new List<string>();
        List<int> m = new List<int>();
        List<string> q = new List<string>();
        String subj = "";
        public First_form()
        {
            InitializeComponent();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void First_form_Load(object sender, EventArgs e)
        {
            

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
                //---------------------------------------------------------------------------------------------------------------------------------
                try
                {
                    myStudentRecognizer = new MEigenRecognizer("" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/holdRecFilePath/thephotos.yml");
                    sqlite_conn = new SQLiteConnection("Data Source=" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/Project.db ; Version =3;");

                    //-------------------------------------------------------------------------
                    //here we ghet the current subject from the databse
                    sqlite_conn.Open();
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    int d1 = datetime.Hour;
                    sqlite_cmd.CommandText = "select subj_id from classRoomT where place = '15' and day = '" + System.DateTime.Now.ToString("dddd") + "' " + "and " + d1 + "between fromT and toT";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        subj = sqlite_datareader.GetString(0);
                    }
                    sqlite_conn.Close();
                    sqlite_datareader.Close();
                    //-------------------------------------------
                    //Now get the Students that study this subject
                    sqlite_conn.Open();
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "select student_id from studentSubjects where subject_id ='" + subj + "';";

                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        m.Add(int.Parse(sqlite_datareader.GetString(0)));
                    }
                    sqlite_conn.Close();
                    sqlite_datareader.Close();

                    //----------------------------------------------
                    //Now we retrieve the photos paths and the labels for the found students

                    for (int i = 0; i < m.Count; ++i)
                    {
                        sqlite_conn.Open();
                        sqlite_cmd = sqlite_conn.CreateCommand();
                        sqlite_cmd.CommandText = "select image_path , student_id from studentsFaces where student_id = " + m[i];

                        sqlite_datareader = sqlite_cmd.ExecuteReader();
                        while (sqlite_datareader.Read())
                        {
                            image.Add(sqlite_datareader.GetString(0));
                            Labels.Add(sqlite_datareader.GetString(1));
                        }
                        sqlite_cmd.ExecuteNonQuery();
                        //close connections
                        sqlite_conn.Close();
                        sqlite_datareader.Close();
                    }
                    //------------------------------------------------------------------------
                    //Now we get the photos saved in the paths ant convert it to Gray images to be able to use it in recognition
                    //the add the labels to to labels holder

                    for (int i = 0; i < image.Count; i++)
                    {
                        //  LoadFaces = "face" + i + ".bmp";
                        trainingImagesHolder.Add(new Image<Gray, byte>(image[i]));
                        labelsHolder.Add(Labels[i]);
                    }

                    //----------------------------------------------------------------------------------------------
                    //Now after we add the training photos and the labels to TrainingImageHolder and LAbelsHolder
                    //we parse labels to intger to be able to path them to the recognizer 

                    integerLablesHolder = new int[labelsHolder.ToArray().Length];
                    for (int i = 0; i < labelsHolder.ToArray().Length; i++)
                    {
                        string[] arrayLablesHolder = labelsHolder.ToArray();
                        Int32.TryParse(arrayLablesHolder[i], out integerLablesHolder[i]);
                    }
                    //--------------------------------------------------------------------------------------------------------------------------------
                }
                catch (Exception ex) {
               // MessageBox.Show(ex.Message);
            }
                try
                {
                    myStudentRecognizer.TrainRecognizer(trainingImagesHolder.ToArray(), integerLablesHolder);
                    Atten attendance = new Atten();
                    attendance.Show();
                    this.Hide();
                }
                catch (Exception error)
                {
                //MessageBox.Show(e.ToString());
                NoLecture no = new NoLecture();
                no.Show();

                }

            }
        
        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            Admin_Login admin = new Admin_Login();
            admin.Show();
            this.Hide();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            LogIn log = new LogIn();
            log.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Help h = new Help();
            h.Show();
            this.Hide();
        }

         public MEigenRecognizer getStudentRecognizer() {
            return myStudentRecognizer;
        }
    }
}
