using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
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
                Console.Out.WriteLine("Initialize git-selfie for current repository");
                
                var sb = new StringBuilder();
                sb.AppendLine(@"#!/bin/sh");
                sb.AppendLine("MESSAGE=$(git log -1 HEAD --pretty=format:%s)");
                sb.AppendLine("SHA=$(git rev-list -1 HEAD)");
                sb.AppendLine("exec \"gitselfie\" \"$MESSAGE\" \"$SHA\"");

                if (Directory.Exists(".git"))
                {
                    File.WriteAllText(@".git\hooks\post-commit", sb.ToString());
                    Console.Out.WriteLine("git-selfie initialized");
                }
                else
                {
                    Console.Out.WriteLine("Could not initialize git-selfie because .git folder was not found");
                }
                
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

            DrawCommitText(bmp);
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

        private static void DrawCommitText(Bitmap bmp)
        {
            DrawMessage(bmp, commit.Message);

            string dateFormat = DateTime.Now.ToString("dd.MM.yyyy hh:ss");
            DrawDate(bmp, dateFormat);
        }

        private static void DrawMessage(Bitmap bmp, string message)
        {
            Graphics g = Graphics.FromImage(bmp);

            //this will center align our text at the bottom of the image
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Far;

            int fontSize = 36;

            //define a font to use.
            Font f = new Font("Impact", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);

            //pen for outline - set width parameter
            Pen p = new Pen(ColorTranslator.FromHtml("#77090C"), 4);
            p.LineJoin = LineJoin.Round; //prevent "spikes" at the path

            //this makes the gradient repeat for each text line
            Rectangle fr = new Rectangle(0, bmp.Height - f.Height, bmp.Width, f.Height);
//            LinearGradientBrush b = new LinearGradientBrush(fr,
//                                                            ColorTranslator.FromHtml("#FF6493"),
//                                                            ColorTranslator.FromHtml("#D00F14"),
//                                                            90);

            var b = new SolidBrush(Color.Snow);

            //this will be the rectangle used to draw and auto-wrap the text.
            //basically = image size
            Rectangle r = new Rectangle(10, 0, bmp.Width-10, bmp.Height);

            GraphicsPath gp = new GraphicsPath();

            //look mom! no pre-wrapping!
            gp.AddString(message,
                f.FontFamily, (int) f.Style, fontSize, r, sf);

            //these affect lines such as those in paths. Textrenderhint doesn't affect
            //text in a path as it is converted to ..well, a path.    
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //TODO: shadow -> g.translate, fillpath once, remove translate
            g.DrawPath(p, gp);
            g.FillPath(b, gp);

            //cleanup
            gp.Dispose();
            b.Dispose();
            b.Dispose();
            f.Dispose();
            sf.Dispose();
            g.Dispose();
        }


        private static void DrawDate(Bitmap bmp, string message)
        {
            Graphics g = Graphics.FromImage(bmp);

            //this will center align our text at the bottom of the image
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;
            sf.LineAlignment = StringAlignment.Near;
            
            int fontSize = 20;

            //define a font to use.
            Font f = new Font("Trebuchet MS", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);

            //pen for outline - set width parameter
            Pen p = new Pen(ColorTranslator.FromHtml("#77090C"), 4);
            p.LineJoin = LineJoin.Round; //prevent "spikes" at the path

            //this makes the gradient repeat for each text line
            Rectangle fr = new Rectangle(0, bmp.Height - f.Height, bmp.Width, f.Height);
            //            LinearGradientBrush b = new LinearGradientBrush(fr,
            //                                                            ColorTranslator.FromHtml("#FF6493"),
            //                                                            ColorTranslator.FromHtml("#D00F14"),
            //                                                            90);

            var b = new SolidBrush(Color.Gainsboro);

            //this will be the rectangle used to draw and auto-wrap the text.
            //basically = image size
            Rectangle r = new Rectangle(0, 10, bmp.Width, bmp.Height);
            
            GraphicsPath gp = new GraphicsPath();

            //look mom! no pre-wrapping!
            gp.AddString(message,
                f.FontFamily, (int)f.Style, fontSize, r, sf);

            //these affect lines such as those in paths. Textrenderhint doesn't affect
            //text in a path as it is converted to ..well, a path.    
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //TODO: shadow -> g.translate, fillpath once, remove translate
            g.DrawPath(p, gp);
            g.FillPath(b, gp);

            //cleanup
            gp.Dispose();
            b.Dispose();
            b.Dispose();
            f.Dispose();
            sf.Dispose();
            g.Dispose();
        }
    }
}
