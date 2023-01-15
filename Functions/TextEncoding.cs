using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtfUnknown;

namespace FormatConverter.Functions
{
    internal static class TextEncoding
    {
        internal static byte[] GetBytes(this string value, Encoding? encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            return encoding.GetBytes(value);
        }

        internal static string GetString(this byte[] value, Encoding? encoding = null)
        {
            if (encoding == null) encoding = CharsetDetector.DetectFromBytes(value).Detected.Encoding;
            return encoding.GetString(value);
        }
    }
}
