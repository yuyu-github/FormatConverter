using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using FormatConverter.Functions;

namespace FormatConverter.FileTypes
{
    internal class Text : FileType
    {
        public override string Name { get; } = "テキスト";
        public override string Id { get; } = "Text";
        public override string[] Extensions { get; } = { "txt" };

        [ConvertMethod]
        public byte[] ToMarkdown(byte[] data)
        {
            string str = data.GetString();
            str = new Regex(@"[\\`*_}\])#+\-=.!~|$]").Replace(str, "\\$0");
            str = str.Replace("&", "&amp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = new Regex("\r?\n").Replace(str, "  \n");
            str = new Regex(@"^ ( {3,})", RegexOptions.Multiline).Replace(str, "&#32;$1");
            return str.GetBytes();
        }
    }
}
