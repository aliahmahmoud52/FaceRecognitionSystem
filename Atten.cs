using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class Atten : Form
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
        MEigenRecognizer myRecognizer;
        SQLiteConnection sqlite_conn, sqlite_conn1, sqlite_conn2;
        SQLiteCommand sqlite_cmd, sqlite_cmd1, sqlite_cmd2;
        SQLiteDataReader sqlite_datareader, sqlite_datareader1;
        List<string> image = new List<string>();
        List<string> Labels = new List<string>();
        List<int> m = new List<int>();
        List<string> q = new List<string>();
        String subj = "";

        public Atten()
        {
            InitializeComponent();
    
            try
            {
                myRecognizer = new MEigenRecognizer("" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/holdRecFilePath/thephotos.yml");
                sqlite_conn = new SQLiteConnection("Data Source=" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/Project.db ; Version =3;");

               
                String s = "";

                DateTime dateTime = new DateTime();

                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();
                int d = datetime.Hour;
                Console.WriteLine(System.DateTime.Now.ToString("dddd"));
                sqlite_cmd.CommandText = "select subj_id from classRoomT where place = '15' and day = '" + System.DateTime.Now.ToString("dddd") + "' " + "and " + d + "between fromT and toT";

                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    subj = sqlite_datareader.GetString(0);
                }
                sqlite_conn.Close();
                sqlite_datareader.Close();
                // Console.WriteLine("subj" +subj);
              /*  if (subj == "") {
                    NoLecture no = new NoLecture();
                    no.Show();
                    this.Hide();
                }*/
                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "select student_id from studentSubjects where subj_id ='" + subj + "';";

                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    m.Add(int.Parse(sqlite_datareader.GetString(0)));
                }


                sqlite_conn.Close();
                sqlite_datareader.Close();



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

                for (int i = 0; i < image.Count; i++)
                {
                    //  LoadFaces = "face" + i + ".bmp";
                    trainingImagesHolder.Add(new Image<Gray, byte>(image[i]));
                    labelsHolder.Add(Labels[i]);
                }
                // sqlite_conn.Close();
                //Now Load the images from the TrainedFaces Folder to our List that the images to be used in the code later
                //And the labels to our List that holds the lables to be used later .

                //Now after wwe added the photos to our training set holder and the lables
                integerLablesHolder = new int[labelsHolder.ToArray().Length];
                for (int i = 0; i < labelsHolder.ToArray().Length; i++)
                {
                    string[] arrayLablesHolder = labelsHolder.ToArray();
                    Int32.TryParse(arrayLablesHolder[i], out integerLablesHolder[i]);
                }
                myRecognizer.TrainRecognizer(trainingImagesHolder.ToArray(), integerLablesHolder);
            }
            catch (Exception ex) { }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            _capture.Dispose();

            Application.Idle -= ProcessFrame;

            ID_Camera nt = new ID_Camera();
            nt.Show();
            this.Hide();
        }
       
        private void Atten_Load(object sender, EventArgs e)
        {
            Run();
        }

        private void Run()
        {
            try
            {
                _capture = new VideoCapture();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Application.Idle += ProcessFrame;

        }
        private void ProcessFrame(object sender, EventArgs e)
        {
            try
            {
                String m = "";

                DateTime datetime = DateTime.Now;
                sqlite_conn = new SQLiteConnection("Data Source=" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/Project.db ; Version =3;");

                int lable = 0;
                _cascadeClassifier = new CascadeClassifier("C:/Emgu/emgucv-windesktop 3.2.0.2682/opencv/data/haarcascades/haarcascade_frontalface_default.xml");
                using (var imageFrame = _capture.QueryFrame().ToImage<Bgr, Byte>())
                {

                    if (imageFrame != null)
                    {
                        var grayframe = imageFrame.Convert<Gray, byte>();
                        var faces = _cascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty); //the actual face detection happens here
                        foreach (var face in faces)
                        {
                            imageFrame.Draw(face, new Bgr(Color.BurlyWood), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them
                            grayframe.Draw(face, new Gray(1.523), 3);
                            unknownFace = grayframe;
                            lable = myRecognizer.RecognizeUser(unknownFace);
                        }

                    }
                    imgCamUser.Image = imageFrame;
                    if (lable == 0)
                    {
                        label4.Text = "Sorry I don't know you , please leave a copy of ur ID";
                    }
                    else
                    {

                        sqlite_conn.Open();
                        sqlite_cmd = sqlite_conn.CreateCommand();
                        sqlite_cmd.CommandText = "select stu_name from student where stu_id =" + lable + ";";
                        sqlite_datareader = sqlite_cmd.ExecuteReader();

                        while (sqlite_datareader.Read())
                        {
                            m = sqlite_datareader.GetString(0);
                        }

                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn.Close();
                        sqlite_datareader.Close();
                        label4.Text = "Hi " + m;

                        String r = "";
                        //  Console.WriteLine("name " + m + "id" + lable);

                        sqlite_conn.Open();
                        sqlite_cmd = sqlite_conn.CreateCommand();
                        sqlite_cmd.CommandText = "select student_id from attendance where student_id =" + lable + "and subj_id = '" + subj + "';";
                        sqlite_datareader = sqlite_cmd.ExecuteReader();

                        while (sqlite_datareader.Read())
                        {
                            r = sqlite_datareader.GetString(0);
                        }

                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_conn.Close();
                        sqlite_datareader.Close();

                        if (lable != 0 && r == "")
                        {

                            sqlite_conn.Open();
                            sqlite_cmd = sqlite_conn.CreateCommand();
                            sqlite_cmd.CommandText = "insert into attendance values(" + lable + ",'" + subj + "','" + datetime + "');";
                            sqlite_cmd.ExecuteNonQuery();
                            sqlite_conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex) { }
            }
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            _capture.Dispose();
            Application.Idle -= ProcessFrame;

            First_form first = new First_form();
            first.Show();
            this.Hide();
        }

        private void imgCamUser_Click(object sender, EventArgs e)
        {

        }
    }
}
