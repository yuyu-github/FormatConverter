using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    internal class Tiff : FileType
    {
        public override string Name { get; } = "TIFF";
        public override string Id { get; } = "TIFF";
        public override string[] Extensions { get; } = { "tiff", "tif" };
    }
}
