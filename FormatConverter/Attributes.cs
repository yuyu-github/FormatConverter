using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ConvertMethodAttribute : Attribute {
        public bool UseInputFilePath = false;
        public bool UseOutputFilePath = false;
    }
}
