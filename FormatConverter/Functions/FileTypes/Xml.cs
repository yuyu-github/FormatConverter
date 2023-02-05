using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading.Tasks;

namespace FormatConverter.Functions.FileTypes
{
    internal static class XmlFunctions
    {
        public static string ConvertToCorrectName(string input)
        {
            return input.Aggregate("", (str, i) =>
            {
                if (str == "")
                {
                    if (!XmlConvert.IsStartNCNameChar(i))
                    {
                        if (Regex.IsMatch(i.ToString(), @"[0-9]")) return "_" + i;
                        else return "_";
                    }
                }
                else if (!XmlConvert.IsNCNameChar(i)) return str + "_";
                return str + i;
            });
        }
    }
}
