using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using AForge.Video;
using AForge.Video.DirectShow;

namespace GitSelfie
{
    class Program
    {
        private static VideoCaptureDevice camera;
        private static string commitMessage;
        private static string commitHash;

        static void Main(string[] args)
        {
            commitMessage = args[0];
            commitHash = args[1];

            if (commitHash.Length > 20)
            {
                commitHash = commitHash.Substring(0, 20);
            }

            var webcamColl = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            camera = new VideoCaptureDevice(webcamColl[0].MonikerString);
            camera.NewFrame += Device_NewFrame;

            Console.WriteLine("Taking git-selfie :)");
            camera.Start();
        }

        static void Device_NewFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap bmp = (Bitmap)e.Frame.Clone();
            DrawMessage(bmp, commitMessage);
            DrawCommitHash(bmp, commitHash);
            bmp.Save("C:\\Foo\\"+Guid.NewGuid()+"_bar.png");
           
         
            camera.SignalToStop();
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
            Font f = new Font("Impact", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);

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
            Rectangle r = new Rectangle(10, 0, bmp.Width, bmp.Height);

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


        private static void DrawCommitHash(Bitmap bmp, string message)
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
