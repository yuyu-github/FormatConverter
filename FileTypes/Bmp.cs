using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using ImageMagick;

using FormatConverter.Functions.FileTypeGroups;

namespace FormatConverter.FileTypes
{
    internal class Bmp : FileType
    {
        public override string Name { get; } = "BMP";
        public override string Id { get; } = "BMP";
        public override string[] Extensions { get; } = { "bmp", "dib" };

        [ConvertMethod]
        public byte[] ToPNG(byte[] data)
        {
            return Image.ChangeFormat(data, ImageFormat.Png);
        }

        [ConvertMethod]
        public byte[] ToJPEG(byte[] data)
        {
            return Image.ChangeFormat(data, ImageFormat.Jpeg);
        }

        [ConvertMethod]
        public byte[] ToGIF(byte[] data)
        {
            return Image.ChangeFormat(data, ImageFormat.Gif);
        }

        [ConvertMethod]
        public byte[] ToTIFF(byte[] data)
        {
            return Image.ChangeFormat(data, ImageFormat.Tiff);
        }

        [ConvertMethod]
        public byte[] ToWebP(byte[] data)
        {
            return Image.ChangeFormat(data, MagickFormat.WebP);
        }
    }
}
