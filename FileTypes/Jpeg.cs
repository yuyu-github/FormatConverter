using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

using FormatConverter.Functions.FileTypeGroups;

namespace FormatConverter.FileTypes
{
    internal class Jpeg : FileType
    {
        public override string Name { get; } = "JPEG";
        public override string Id { get; } = "JPEG";
        public override string[] Extensions { get; } = { "jpg", "jpeg", "jpe", "jfif", "jfi", "jif" };

        [ConvertMethod]
        public byte[] ToPNG(byte[] data)
        {
            return Image.ChangeFormat(data, ImageFormat.Png);
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
        public byte[] ToBMP(byte[] data)
        {
            return Image.ChangeFormat(data, ImageFormat.Bmp);
        }

        [ConvertMethod]
        public byte[] ToWebP(byte[] data)
        {
            return Image.ChangeFormatToWebP(data);
        }
    }
}
