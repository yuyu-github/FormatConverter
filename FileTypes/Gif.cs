using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    internal class Gif : FileType
    {
        public override string Name { get; } = "GIF";
        public override string Id { get; } = "GIF";
        public override string[] Extensions { get; } = { "gif" };
    }
}
