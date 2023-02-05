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

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToPNG(string input, string output)
        {
            Image.ChangeFormat(input, output, MagickFormat.Png);
        }

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToJPEG(string input, string output)
        {
            Image.ChangeFormat(input, output, MagickFormat.Jpeg);
        }

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToGIF(string input, string output)
        {
            Image.ChangeFormat(input, output, MagickFormat.Gif);
        }

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToTIFF(string input, string output)
        {
            Image.ChangeFormat(input, output, MagickFormat.Tiff);
        }

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToBMP(string input, string output)
        {
            Image.ChangeFormat(input, output, MagickFormat.Bmp);
        }

        [ConvertMethod(UseInputFilePath = true, UseOutputFilePath = true)]
        public void ToWebP(string input, string output)
        {
            Image.ChangeFormat(input, output, MagickFormat.WebP);
        }
    }
}
