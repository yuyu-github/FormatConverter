using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter
{
    [Serializable()]
    public class ConversionException : Exception
    {
        public ConversionException() : base() { }
        public ConversionException(string message) : base(message) { }
        public ConversionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
