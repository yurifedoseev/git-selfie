using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GitSelfie
{
    public class CommitDrawing
    {
        private readonly Bitmap bmp;
        private readonly Commit commit;

        public static void Draw(Bitmap bmp, Commit commit)
        {
            var drawing = new CommitDrawing(bmp, commit);
            drawing.Draw();
        }

        private CommitDrawing(Bitmap bmp, Commit commit)
        {
            this.bmp = bmp;
            this.commit = commit;
        }

        public void Draw()
        {
            DrawCommitMessage();
            DrawCommitTimestamp();
        }

        private void DrawCommitMessage()
        {
            var textSettings = new TextSettings
            {
                Font = new Font("Helvetica", 36, FontStyle.Bold, GraphicsUnit.Pixel),
                StringFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far }
            };
            DrawText(commit.Message, textSettings);
            textSettings.Dispose();
        }

        private void DrawCommitTimestamp()
        {
            var settings = new TextSettings
            {
                Font = new Font("Helvetica", 20, FontStyle.Bold, GraphicsUnit.Pixel),
                StringFormat = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}
            };
            DrawText(DateTime.Now.ToString("dd.MM.yyyy hh:ss"), settings);
            settings.Dispose();
        }

        private void DrawText(string message, TextSettings textSettings)
        {
            Rectangle drawRectangle = new Rectangle(10, 10, bmp.Width-10, bmp.Height);
            Graphics g = Graphics.FromImage(bmp);
            Font f = textSettings.Font;
            Pen p = new Pen(ColorTranslator.FromHtml("#77090C"), 4) { LineJoin = LineJoin.Round };
            var b = new SolidBrush(Color.Gainsboro);

            GraphicsPath gp = new GraphicsPath();

            gp.AddString(message, f.FontFamily, (int)f.Style, f.Size, drawRectangle, textSettings.StringFormat);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawPath(p, gp);
            g.FillPath(b, gp);

            //cleanup
            gp.Dispose();
            b.Dispose();
            b.Dispose();
            g.Dispose();
        } 
    }
}