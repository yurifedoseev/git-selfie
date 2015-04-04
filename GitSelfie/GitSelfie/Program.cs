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

        static void Main(string[] args)
        {
            var webcamColl = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            camera = new VideoCaptureDevice(webcamColl[0].MonikerString);
            camera.NewFrame += Device_NewFrame;
            camera.Start();
            Console.ReadLine();
        }

        static void Device_NewFrame(object sender, NewFrameEventArgs e)
        {
            Bitmap bmp = (Bitmap)e.Frame.Clone();
            RectangleF rectf = new RectangleF(0, bmp.Size.Height - bmp.Size.Height / 3, (int)(bmp.Size.Width*0.95), bmp.Size.Height / 3);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Far;
           
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString("Новый биллинг в партнерке, фикс всех багов", new Font("Arial", 32), Brushes.White, rectf, stringFormat);
            
            g.Flush();

            bmp.Save("C:\\Foo\\"+Guid.NewGuid()+"_bar.png");
            Console.WriteLine("Snapshot Saved.");
         
            Console.WriteLine("Stopping ...");
            camera.SignalToStop();
            Console.WriteLine("Stopped .");
        }
    }
}
