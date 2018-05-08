using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Face;

namespace Project_1_1
{
   public class MEigenRecognizer
    {

        private EigenFaceRecognizer _faceRecognizer;
        private String _recognizerFilePath;

        public MEigenRecognizer(String recognizerFilePath)
        {
            _recognizerFilePath = recognizerFilePath;
            _faceRecognizer = new EigenFaceRecognizer();
        }

        public bool TrainRecognizer(Image<Gray, byte>[] faceImages, int[] faceLabels)
        {

            _faceRecognizer.Train(faceImages, faceLabels);
            _faceRecognizer.Save(_recognizerFilePath);

            return true;

        }

        public void LoadRecognizerData()
        {
            _faceRecognizer.Load(_recognizerFilePath);
        }

        public int RecognizeUser(Image<Gray, byte> userImage)
        {

            _faceRecognizer.Load(_recognizerFilePath);
            var result = _faceRecognizer.Predict(userImage.Resize(100, 100, Inter.Cubic));
            Console.WriteLine(result.Distance);
            if (result.Distance > 0 && result.Distance< 3000 )
            {
                return result.Label;
            }
            return 0;
        }

    }
}