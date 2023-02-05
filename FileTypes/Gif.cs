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
    internal class Gif : FileType
    {
        public override string Name { get; } = "GIF";
        public override string Id { get; } = "GIF";
        public override string[] Extensions { get; } = { "gif" };

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
            return Image.ChangeFormat(data, MagickFormat.WebP);
        }

        [ConvertMethod]
        public byte[] ToSVG(byte[] data)
        {
            return Image.ChangeFormat(data, MagickFormat.Svg);
        }
    }
}
