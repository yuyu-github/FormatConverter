﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;

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

        public static byte[] ChangeFormatToWebP(byte[] data)
        {
            var factory = new ImageFactory();
            factory.Load(data);
            factory.Format(new WebPFormat());

            var stream = new MemoryStream();
            factory.Save(stream);
            return stream.ToArray();
        }
    }
}
