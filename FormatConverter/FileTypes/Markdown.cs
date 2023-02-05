using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FormatConverter.Functions;

namespace FormatConverter.FileTypes
{
    internal class Markdown : FileType
    {
        public override string Name { get; } = "Markdown";
        public override string Id { get; } = "Markdown";
        public override string[] Extensions { get; } = { "md" };

        [ConvertMethod]
        public string ToHTML(string data)
        {
            return Markdig.Markdown.ToHtml(data);
        }

        [ConvertMethod]
        public string ToText(string data)
        {
            return Markdig.Markdown.ToPlainText(data);
        }
    }
}
