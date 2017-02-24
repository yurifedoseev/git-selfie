using System;
using System.Drawing;
using System.IO;
using AForge.Video;
using AForge.Video.DirectShow;
using GitSelfie.Domain.Interfaces;
using GitSelfie.Domain.Models;
using GitSelfie.Helpers;

namespace GitSelfie.Commands
{
    public class SelfieCommand : ICommand
    {
        private readonly Commit commit;
        private VideoCaptureDevice camera;

        public SelfieCommand(Commit commit)
        {
            this.commit = commit;
        }

        public void Execute()
        {
            var webcamColl = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (webcamColl.Count < 1)
            {
                Console.Out.WriteLine("Webcam was not found.");
                return;
            }

            camera = new VideoCaptureDevice(webcamColl[0].MonikerString);
            camera.NewFrame += Device_NewFrame;

            Console.WriteLine("Taking git-selfie :)");
            camera.Start();
        }

        void Device_NewFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap bmp = (Bitmap)e.Frame.Clone();
            camera.SignalToStop();
            CommitDrawing.Draw(bmp, commit);

            SaveImage(bmp);
        }

        private static void SaveImage(Bitmap bmp)
        {
            string savePath = GetFileNameToSave();
            bmp.Save(savePath);
        }

        private static string GetFileNameToSave()
        {
            string folderPath = GetSaveFolderPath();
            string fileName = DateTime.Now.ToString("yyyy_MM_dd_hhmmss") + ".png";
            return Path.Combine(folderPath, fileName);
        }

        private static string GetSaveFolderPath()
        {
            string myPicturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            const string folderName = "git-selfie";
            string targetFolder = Path.Combine(myPicturesPath, folderName);
            if (Directory.Exists(targetFolder) == false)
            {
                Directory.CreateDirectory(targetFolder);
            }
            return targetFolder;
        }
    }
}