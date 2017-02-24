using System;
using System.Drawing;

namespace GitSelfie.Helpers
{
    public class TextOptions : IDisposable
    {
        public StringFormat StringFormat { get; set; }

        public Font Font { get; set; }

        public void Dispose()
        {
            StringFormat?.Dispose();

            Font?.Dispose();
        }
    }
}