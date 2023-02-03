using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FormatConverter.Functions.FileTypeGroups
{
    internal static class Image
    {
        public static byte[] ChangeFormat(byte[] data, ImageFormat format)
        {
            var bitmap = new Bitmap(new MemoryStream(data));
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, format);
                return stream.ToArray();
            }
        }
    }
}
