using System;
using System.Drawing;
using System.IO;
using AForge.Video;
using AForge.Video.DirectShow;

namespace GitSelfie
{
    class Program
    {
        private static VideoCaptureDevice camera;
        private static Commit commit;

        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0].ToLower() == "init")
            {
                PostCommitHook.Initialize();            
                return;
            }

            if (args.Length >= 2)
            {
                commit = LoadCommitData(args);
            }

            var webcamColl = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            camera = new VideoCaptureDevice(webcamColl[0].MonikerString);
            camera.NewFrame += Device_NewFrame;

            Console.WriteLine("Taking git-selfie :)");
            camera.Start();
        }

        private static Commit LoadCommitData(string[] args)
        {
            var data = new Commit
            {
                Message = args[0],
                Sha1 = args[1]
            };

            if (data.Sha1.Length > 20)
            {
                data.Sha1 = data.Sha1.Substring(0, 20);
            }

            return data;
        }

        static void Device_NewFrame(object sender, NewFrameEventArgs e)
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
