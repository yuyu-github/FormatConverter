using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    internal class YAML : FileType
    {
        public override string Name { get; } = "YAML";
        public override string Id { get; } = "YAML";
        public override string[] Extensions { get; } = { "yml", "yaml" };
    }
}
