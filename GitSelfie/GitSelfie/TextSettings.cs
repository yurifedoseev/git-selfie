using System;
using System.Drawing;

namespace GitSelfie
{
    public class TextSettings : IDisposable
    {
        public StringFormat StringFormat { get; set; }

        public Font Font { get; set; }

        public void Dispose()
        {
            if (StringFormat != null)
            {
                StringFormat.Dispose();
            }

            if (Font != null)
            {
                Font.Dispose();
            }
        }
    }
}