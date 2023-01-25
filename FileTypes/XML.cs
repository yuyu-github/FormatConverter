using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    internal class Xml : FileType
    {
        public override string Name { get; } = "XML";
        public override string Id { get; } = "XML";
        public override string[] Extensions { get; } = { "xml" };
    }
}
