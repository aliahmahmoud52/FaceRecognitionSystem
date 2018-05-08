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

namespace Project_1_1
{

    public partial class ID_Camera : Form
    {
        VideoCapture _capture;
        System.Timers.Timer t;
        int s = 0;
        public ID_Camera()
        {
            InitializeComponent();
            
        }

        private void ID_Camera_Load(object sender, EventArgs e)
        {
            t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += OnTimeEvent;
            t.Start();
            Run();

        }


        private void OnTimeEvent(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (s == 5)
                {
                    _capture.QueryFrame().Save(Application.StartupPath + "bb.bmp");
                    t.Stop();
                }
                else
                {
                    s++;
                    label1.Text = string.Format("{0}", s.ToString());
                }
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
           
            imgCamUser.Image = _capture.QueryFrame();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
