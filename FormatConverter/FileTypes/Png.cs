using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;
using ImageMagick;

using FormatConverter.Functions.FileTypeGroups;

namespace FormatConverter.FileTypes
{
    internal class Png : FileType
    {
        public override string Name { get; } = "PNG";
        public override string Id { get; } = "PNG";
        public override string[] Extensions { get; } = { "png" };

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToJPEG(string input, string output)
        {
            Image.ChangeFormat(input, output, ImageFormat.Jpeg);
        }

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToGIF(string input, string output)
        {
            Image.ChangeFormat(input, output, ImageFormat.Gif);
        }

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToTIFF(string input, string output)
        {
            Image.ChangeFormat(input, output, ImageFormat.Tiff);
        }

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToBMP(string input, string output)
        {
            Image.ChangeFormat(input, output, ImageFormat.Bmp);
        }

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToWebP(string input, string output)
        {
            Image.ChangeFormat(input, output, MagickFormat.WebP);
        }

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToSVG(string input, string output)
        {
            Image.ChangeFormat(input, output, MagickFormat.Svg);
        }
    }
}
