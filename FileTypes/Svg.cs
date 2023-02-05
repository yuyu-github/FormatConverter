using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;

using FormatConverter.Functions.FileTypeGroups;

namespace FormatConverter.FileTypes
{
    internal class Svg : FileType
    {
        public override string Name { get; } = "SVG";
        public override string Id { get; } = "SVG";
        public override string[] Extensions { get; } = { "svg" };

        [ConvertMethod]
        public byte[] ToPNG(byte[] data)
        {
            return Image.ChangeFormat(data, MagickFormat.Png);
        }

        [ConvertMethod]
        public byte[] ToJPEG(byte[] data)
        {
            return Image.ChangeFormat(data, MagickFormat.Jpeg);
        }

        [ConvertMethod]
        public byte[] ToGIF(byte[] data)
        {
            return Image.ChangeFormat(data, MagickFormat.Gif);
        }

        [ConvertMethod]
        public byte[] ToTIFF(byte[] data)
        {
            return Image.ChangeFormat(data, MagickFormat.Tiff);
        }

        [ConvertMethod]
        public byte[] ToBMP(byte[] data)
        {
            return Image.ChangeFormat(data, MagickFormat.Bmp);
        }

        [ConvertMethod]
        public byte[] ToWebP(byte[] data)
        {
            return Image.ChangeFormat(data, MagickFormat.WebP);
        }
    }
}
