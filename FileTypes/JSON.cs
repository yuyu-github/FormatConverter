using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    internal class JSON : FileType
    {
        public override string Name { get; } = "JSON";
        public override string Id { get; } = "JSON";
        public override string[] Extensions { get; } = { "json" };
    }
}
