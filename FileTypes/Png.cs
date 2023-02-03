using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

using FormatConverter.Functions.FileTypeGroups;

namespace FormatConverter.FileTypes
{
    internal class Png : FileType
    {
        public override string Name { get; } = "PNG";
        public override string Id { get; } = "PNG";
        public override string[] Extensions { get; } = { "png" };

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
        public byte[] ToBMP(byte[] data)
        {
            return Image.ChangeFormat(data, ImageFormat.Bmp);
        }
    }
}
