using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    public abstract class FileType
    {
        public abstract string Id { get; }
        public abstract string[] Extensions { get; }

        public bool IsConvertible(string id)
        {
            return GetType().GetMethod("To" + id) != null;
        }
    }
}
