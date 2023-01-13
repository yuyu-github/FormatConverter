using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    internal class Text : FileType
    {
        public override string Id { get; } = "Text";
        public override string[] Extensions { get; } = { "txt" };
    }
}
