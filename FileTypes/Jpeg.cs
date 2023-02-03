using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    internal class Jpeg : FileType
    {
        public override string Name { get; } = "JPEG";
        public override string Id { get; } = "JPEG";
        public override string[] Extensions { get; } = { "jpg", "jpeg", "jpe", "jfif", "jfi", "jif" };
    }
}
