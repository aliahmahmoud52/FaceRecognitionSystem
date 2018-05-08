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


namespace Project_1_1
{
    public partial class LogIn : Form
    {
        Image<Gray, byte> TrainedFace = null, unknownFace = null;
        private VideoCapture _capture;
        private CascadeClassifier _cascadeClassifier;
        private CascadeClassifier trainCascadeClassifier;
        List<Image<Gray, byte>> trainingImagesHolder = new List<Image<Gray, byte>>();
        List<string> labelsHolder = new List<string>();
        int[] integerLablesHolder;
        int numberOfLables, CountTrainedFaces;
        MEigenRecognizer myRecognizer;
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;
        List<string> image = new List<string>();
        List<string> Labels = new List<string>();
        int lable = 0;

        public LogIn()
        {
            InitializeComponent();

            try
            {
                myRecognizer = new MEigenRecognizer("" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/holdRecFilePath/thephotos.yml");
                sqlite_conn = new SQLiteConnection("Data Source=" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/Project.db ; Version =3;");

                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();
                //create command
                sqlite_cmd.CommandText = "select image_path , doctor_id from doctorsFaces ;";
                //datareader to read from db
                sqlite_datareader = sqlite_cmd.ExecuteReader();

                while (sqlite_datareader.Read())
                {
                    image.Add(sqlite_datareader.GetString(0));
                    Labels.Add(sqlite_datareader.GetString(1));
                }


                sqlite_cmd.ExecuteNonQuery();
                sqlite_conn.Close();
                sqlite_datareader.Close();
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                pictureBox1_Click(this, new EventArgs()); return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                sqlite_conn.Open();
                String s = "";
                sqlite_cmd = sqlite_conn.CreateCommand();
                //create command
                sqlite_cmd.CommandText = "select password from doctor where doc_id = " + lable + ";";
                //datareader to read from db
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                //MessageBox.Show(sqlite_cmd.ExecuteReader()+"");
                while (sqlite_datareader.Read())
                {
                    s = sqlite_datareader.GetString(0);

                }
                sqlite_cmd.ExecuteNonQuery();
                sqlite_conn.Close();
                sqlite_datareader.Close();

                if (textBox1.Text == s)
                {
                    Extract ex = new Extract();
                    ex.setId(lable);
                    ex.Show();
                    this.Hide();
                    _capture.Dispose();
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    invalid2 n = new invalid2();
                    n.Show();
                    this.Hide();
                    _capture.Dispose();
                    Application.Idle -= ProcessFrame;
                }
            }
            catch (Exception ex) { }
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            textBox1.PasswordChar = '*';
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
                    sqlite_conn.Open();
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_cmd.CommandText = "select doc_name from doctor where doc_id =" + lable + ";";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();

                    while (sqlite_datareader.Read())
                    {
                        m = sqlite_datareader.GetString(0);
                    }

                    sqlite_cmd.ExecuteNonQuery();
                    sqlite_conn.Close();
                    sqlite_datareader.Close();
                    Console.WriteLine("name " + m + "id" + lable);
                    label4.Text = "Hi " + m;

                }
            }
            catch (Exception ex) { }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

            First_form ft = new First_form();
            ft.Show();
            this.Hide();
            _capture.Dispose();
            Application.Idle -= ProcessFrame;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Notme not = new Notme();
            not.Show();
            this.Hide();
            _capture.Dispose();
            Application.Idle -= ProcessFrame;
        }

        private void imgCamUser_Click(object sender, EventArgs e)
        {

        }
    }
}