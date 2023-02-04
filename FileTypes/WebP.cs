using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

using FormatConverter.Functions.FileTypeGroups;

namespace FormatConverter.FileTypes
{
    internal class WebP : FileType
    {
        public override string Name { get; } = "WebP";
        public override string Id { get; } = "WebP";
        public override string[] Extensions { get; } = { "webp" };
    }
}
