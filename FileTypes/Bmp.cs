using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    internal class Bmp : FileType
    {
        public override string Name { get; } = "BMP";
        public override string Id { get; } = "BMP";
        public override string[] Extensions { get; } = { "bmp", "dib" };
    }
}
