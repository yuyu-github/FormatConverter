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

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToPNG(string input, string output)
        {
            Image.ChangeFormat(input, output, ImageFormat.Png);
        }

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToJPEG(string input, string output)
        {
            Image.ChangeFormat(input, output, ImageFormat.Jpeg);
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
