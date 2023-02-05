using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageMagick;

namespace FormatConverter.Functions.FileTypeGroups
{
    internal static class Image
    {
        public static void ChangeFormat(string input, string output, ImageFormat format)
        {
            var bitmap = new Bitmap(input);
            bitmap.Save(output, format);
        }

        public static void ChangeFormat(string input, string output, MagickFormat format)
        {
            var image = new MagickImage(input);
            image.Write(output, format);
        }
    }
}
