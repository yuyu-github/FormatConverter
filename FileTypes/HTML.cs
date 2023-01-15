using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    internal class HTML : FileType
    {
        public override string Name { get; } = "HTML";
        public override string Id { get; } = "HTML";
        public override string[] Extensions { get; } = { "html", "htm" };
    }
}
