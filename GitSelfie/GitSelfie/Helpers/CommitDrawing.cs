using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using GitSelfie.Domain.Models;

namespace GitSelfie.Helpers
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
            var textSettings = new TextOptions
            {
                Font = new Font("Helvetica", 36, FontStyle.Bold, GraphicsUnit.Pixel),
                StringFormat = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far}
            };
            DrawText(commit.Message, textSettings);
            textSettings.Dispose();
        }

        private void DrawCommitTimestamp()
        {
            var settings = new TextOptions
            {
                Font = new Font("Helvetica", 20, FontStyle.Bold, GraphicsUnit.Pixel),
                StringFormat = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}
            };
            DrawText(DateTime.Now.ToString("dd.MM.yyyy hh:ss"), settings);
            settings.Dispose();
        }

        private void DrawText(string message, TextOptions options)
        {
            using (var gp = InitGraphicsPath(message, options))
            {
                DrawOnGraphicsPath(gp);
            }
        }

        private GraphicsPath InitGraphicsPath(string message, TextOptions options)
        {
            GraphicsPath gp = new GraphicsPath();
            Rectangle r = new Rectangle(10, 10, bmp.Width - 10, bmp.Height);
            Font f = options.Font;
            gp.AddString(message, f.FontFamily, (int)f.Style, f.Size, r, options.StringFormat);
            return gp;
        }

        private void DrawOnGraphicsPath(GraphicsPath gp)
        {
            Graphics graphics = CreateGraphics();

            var pen = new Pen(ColorTranslator.FromHtml("#77090C"), 4) {LineJoin = LineJoin.Round};
            var brush = new SolidBrush(Color.Gainsboro);

            graphics.DrawPath(pen, gp);
            graphics.FillPath(brush, gp);
            
            pen.Dispose();
            brush.Dispose();
            graphics.Dispose();
        }

        private Graphics CreateGraphics()
        {
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            return graphics;
        }
    }
}