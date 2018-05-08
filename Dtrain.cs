using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;



using Emgu.CV;                  // usual Emgu Cv imports
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Finisar.SQLite;
namespace Project_1_1
{
    public partial class Dtrain : Form
    {
        System.Timers.Timer t;
        int s = 0, n = 0;
        String lab1 = "";
        
        private VideoCapture _capture;
        private CascadeClassifier _cascadeClassifier;
        Image<Gray, byte> TrainedFace = null, unknownFace = null;
        private CascadeClassifier trainCascadeClassifier;
        List<Image<Gray, byte>> trainingImagesHolder = new List<Image<Gray, byte>>();
        List<string> labelsHolder = new List<string>();
        int[] integerLablesHolder;
        int numberOfLables, CountTrainedFaces;
        MEigenRecognizer myRecognizer;
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Student")
            {
                textBox3.Visible = false;
                label3.Visible = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            t.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        List<string> image = new List<string>();
        List<string> Labels = new List<string>();

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                pictureBox1_Click(this, new EventArgs()); return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        public Dtrain()
        {
            InitializeComponent();
            try
            {
                myRecognizer = new MEigenRecognizer("" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/holdRecFilePath/thephotos.yml");
            CountTrainedFaces = image.Count;

            sqlite_conn = new SQLiteConnection("Data Source=" + Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/Project.db ; Version =3;");
            sqlite_conn.Open();
            //store old images 
            sqlite_cmd = sqlite_conn.CreateCommand();
            //create command
            sqlite_cmd.CommandText = "select image_path , student_id from studentsFaces ;";
            //datareader to read from db
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                image.Add(sqlite_datareader.GetString(0));
                Labels.Add(sqlite_datareader.GetString(1));
            }
            CountTrainedFaces = image.Count;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_conn.Close();
            sqlite_datareader.Close();
          
                //First get the lables of the images in the training set where the lables are the names of users
                //string getLabels = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                // numberOfLables = Convert.ToInt16(Labels[0]);


                CountTrainedFaces = image.Count;


                //datareader to re

                //Now Load the images from the TrainedFaces Folder to our List that the images to be used in the code later
                //And the labels to our List that holds the lables to be used later .
                for (int i = 0; i <= image.Count; i++)
                {
                    //  LoadFaces = "face" + i + ".bmp";
                    trainingImagesHolder.Add(new Image<Gray, byte>(image[i]));
                    labelsHolder.Add(Labels[i]);
                }
            }
            catch (Exception e)
            {

            }

        }


        private void Dtrain_Load(object sender, EventArgs e)
        {
            
            t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += OnTimeEvent;

            comboBox1.Items.Add("Doctor");
            comboBox1.Items.Add("Student");
           
            Run();
        }


        private void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {

                if (n < 10)
                {

                    if (n == 0 && s == 0) {
                        try
                        {
                            sqlite_conn.Open();
                            sqlite_cmd = sqlite_conn.CreateCommand();
                            //write info of stu/doc
                            if (comboBox1.Text == "Student")
                            {
                                sqlite_cmd.CommandText = "insert into student values(" + int.Parse(textBox2.Text) + ",'" + textBox1.Text + "');";
                            }
                            if (comboBox1.Text == "Doctor")
                            {
                                sqlite_cmd.CommandText = "insert into doctor values(" + int.Parse(textBox2.Text) + ",'" + textBox1.Text + "','" + textBox3.Text + "');";
                            }

                            //datareader to read from db

                            sqlite_cmd.ExecuteNonQuery();
                            sqlite_conn.Close();
                        }
                        catch (Exception ex) { }
                    }

                    // write paths in db
                    

                    if (s == 5)
                    {

                        try
                        {
                            trainCascadeClassifier = new CascadeClassifier("C:/Emgu/emgucv-windesktop 3.2.0.2682/opencv/data/haarcascades/haarcascade_frontalface_default.xml");
                            using (var currentFrame = _capture.QueryFrame().ToImage<Bgr, Byte>())
                            {

                                if (currentFrame != null)
                                {
                                    var grayframe = currentFrame.Convert<Gray, byte>();
                                    var faces = trainCascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty); //the actual face detection happens here
                                    foreach (var face in faces)
                                    {
                                        grayframe.Draw(face, new Gray(1.523), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them

                                    }
                                    grayframe = grayframe.Resize(100, 100, Inter.Cubic);
                                    TrainedFace = grayframe;
                                }

                            }
                            trainingImagesHolder.Add(TrainedFace);
                            labelsHolder.Add(textBox2.Text);

                            sqlite_conn.Open();
                            sqlite_cmd = sqlite_conn.CreateCommand();
                            if (comboBox1.Text == "Student")
                            {
                                lab1 = Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/TrainedFaces/" + textBox2.Text + trainingImagesHolder.Count;
                                trainingImagesHolder.ToArray()[trainingImagesHolder.Count - 1].Save(lab1 + ".bmp");
                                sqlite_cmd.CommandText = "insert into studentsFaces values(" + int.Parse(textBox2.Text) + ",'" + lab1 + ".bmp');";
                            }
                            if (comboBox1.Text == "Doctor")
                            {
                                lab1 = Environment.GetEnvironmentVariable("appdata") + "/Project_1_1/DTrainedFaces/" + textBox2.Text + trainingImagesHolder.Count;
                                trainingImagesHolder.ToArray()[trainingImagesHolder.Count - 1].Save(lab1 + ".bmp");
                                sqlite_cmd.CommandText = "insert into doctorsFaces values(" + int.Parse(textBox2.Text) + ",'" + lab1 + ".bmp');";
                            }
                            sqlite_cmd.ExecuteNonQuery();
                            sqlite_conn.Close();

                        }
                        catch (Exception ex) { }

                                    s = 0;
                                    n++;
                                    label2.Text = "Picture " + n + " Captured";
                                }
                                else s++;
                            }
                            else
                            {
                                t.Stop();
                                  n = 0;
                                s = 0;
                    //MessageBox.Show("Done for this student!");
                    _capture.Dispose();
                    Application.Idle -= ProcessFrame;

                    PicBox pb = new PicBox();
                    pb.Show();
                    this.Hide();
                            }

                            timecont.Text = string.Format("{0}", s.ToString());
                        }));
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
            //   _capture = new VideoCapture();
            try
            {
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

                        }
                    }

                    imgCamUser.Image = imageFrame;

                }
                //  imgCamUser.Image = capture.QueryFrame();
                //trainingImagesHolder.ToArray()[trainingImagesHolder.Count - 1].Save(Application.StartupPath + "/DTrainedFaces/" + trainingImagesHolder.Count + ".bmp");
            }
            catch (Exception ex) { }
            }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            info_train Ff = new info_train();
            Ff.Show();
            this.Hide();
            try
            {
                _capture.Dispose();
            }
            catch (Exception ex) { }
            Application.Idle -= ProcessFrame;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Student") {
                textBox3.Hide();
                label3.Hide();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            add ad = new add();
            ad.Show();
            this.Hide();
        }

        private void imgCamUser_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            First_form Firstform = new First_form();
            Firstform.Show();
            this.Hide();
        }


        //----------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// /we dont use this button-----------------------------------
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogIn_Click(object sender, EventArgs e)
        {
            try
            {
                sqlite_conn.Open();
                sqlite_cmd = sqlite_conn.CreateCommand();
                //write info of stu/doc
                if (comboBox1.Text == "Student")
                {
                    MessageBox.Show("stu");
                    sqlite_cmd.CommandText = "insert into student values(" + int.Parse(textBox2.Text) + ",'" + textBox1.Text + "');";
                }
                if (comboBox1.Text == "Doctor")
                {
                    sqlite_cmd.CommandText = "insert into doctor values(" + int.Parse(textBox2.Text) + ",'" + textBox1.Text + "','" + textBox3.Text + "');";
                }

                //datareader to read from db

                sqlite_cmd.ExecuteNonQuery();
                sqlite_conn.Close();
            }
            catch (Exception ex) { }
            /*
            try
            {
                trainCascadeClassifier = new CascadeClassifier("C:/Emgu/emgucv-windesktop 3.2.0.2682/opencv/data/haarcascades/haarcascade_frontalface_default.xml");
                using (var currentFrame = _capture.QueryFrame().ToImage<Bgr, Byte>())
                {

                    if (currentFrame != null)
                    {
                        var grayframe = currentFrame.Convert<Gray, byte>();
                        var faces = trainCascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty); //the actual face detection happens here
                        foreach (var face in faces)S
                        {
                            grayframe.Draw(face, new Gray(1.523), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them

                        }
                        grayframe = grayframe.Resize(100, 100, Inter.Cubic);
                        TrainedFace = grayframe;
                    }

                }
                trainingImagesHolder.Add(TrainedFace);
                labelsHolder.Add(textBox1.Text);
                /*

                //Show face added in gray scale
                //  imgCamUser.Image = TrainedFace;
                // sqlite_cmd = sqlite_conn1.CreateCommand();


                trainingImagesHolder.ToArray()[trainingImagesHolder.Count - 1].Save(Application.StartupPath + "/DTrainedFaces/" + trainingImagesHolder.Count + ".bmp");

                //connection for count images
                sqlite_conn2 = new SQLiteConnection("Data Source=C:/Users/Rana/Documents/Newfolder/Project_1_1/Project.db ; Version =3;");
                sqlite_conn2.Open();
                sqlite_cmd2 = sqlite_conn2.CreateCommand();
                int x = 0;
                sqlite_cmd2.CommandText = "select count(doctor_id) from doctorsFaces where doctor_id =" + int.Parse(textBox1.Text) + ";";
                sqlite_datareader1 = sqlite_cmd2.ExecuteReader();
                while (sqlite_datareader1.Read())
                {
                    x = int.Parse(sqlite_datareader1.GetString(0));
                }

                Console.WriteLine("count " + x);
                sqlite_conn2.Close();
                sqlite_datareader1.Close();
                if (x >= 10)
                {
                    MessageBox.Show("finished 10 images");


                }
                else
                {
                    //connection for insert3
                    if (x == 0)
                    {
                        sqlite_conn4 = new SQLiteConnection("Data Source=C:/Users/Rana/Documents/Newfolder/Project_1_1/Project.db ; Version =3;");
                        sqlite_conn4.Open();
                        sqlite_cmd4 = sqlite_conn4.CreateCommand();
                        sqlite_cmd4.CommandText = "insert into doctor values (" + int.Parse(textBox1.Text) + ",'" + textBox2.Text + "','" + textBox3.Text + "')";
                        sqlite_cmd4.ExecuteNonQuery();
                        sqlite_conn4.Close();
                    }

                    sqlite_conn5 = new SQLiteConnection("Data Source=C:/Users/Rana/Documents/Newfolder/Project_1_1/Project.db ; Version =3;");
                    sqlite_conn5.Open();
                    sqlite_cmd5 = sqlite_conn5.CreateCommand();
                    sqlite_cmd5.CommandText = "insert into doctorSubjects values (" + int.Parse(textBox1.Text) + ",'" + textBox4.Text + "')";
                    sqlite_cmd5.ExecuteNonQuery();
                    sqlite_conn5.Close();

                    sqlite_conn1 = new SQLiteConnection("Data Source=C:/Users/Rana/Documents/Newfolder/Project_1_1/Project.db ; Version =3;");
                    sqlite_conn1.Open();
                    sqlite_cmd1 = sqlite_conn1.CreateCommand();
                    sqlite_cmd1.CommandText = "insert into doctorsFaces values (" + int.Parse(textBox1.Text) + ",'" + Application.StartupPath + "\\DTrainedFaces\\" + trainingImagesHolder.Count + ".bmp')";
                    sqlite_cmd1.ExecuteNonQuery();
                    sqlite_conn1.Close();
                    MessageBox.Show(textBox1.Text + "´s face detected and added :)", "Training OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch
            {
                MessageBox.Show("Enable the face detection first", "Training Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            //----------------------------------------------------------------------------------------------------------------
            //Now after we added the new images and lables to our training set we will train our eigen recognizer
            integerLablesHolder = new int[labelsHolder.ToArray().Length];
            for (int i = 0; i < labelsHolder.ToArray().Length; i++)
            {
                string[] arrayLablesHolder = labelsHolder.ToArray();
                // integerLablesHolder[i]= Convert.ToInt32(arrayLablesHolder[i]);
                Int32.TryParse(arrayLablesHolder[i], out integerLablesHolder[i]);

            }
            myRecognizer.TrainRecognizer(trainingImagesHolder.ToArray(), integerLablesHolder); 
       */

        }

    }
}
