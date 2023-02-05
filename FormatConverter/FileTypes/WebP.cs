using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;

using FormatConverter.Functions.FileTypeGroups;

namespace FormatConverter.FileTypes
{
    internal class WebP : FileType
    {
        public override string Name { get; } = "WebP";
        public override string Id { get; } = "WebP";
        public override string[] Extensions { get; } = { "webp" };

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
        public void ToSVG(string input, string output)
        {
            Image.ChangeFormat(input, output, MagickFormat.Svg);
        }
    }
}
