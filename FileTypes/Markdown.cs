using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig;

using FormatConverter.Functions;

namespace FormatConverter.FileTypes
{
    internal class Markdown : FileType
    {
        public override string Name { get; } = "Markdown";
        public override string Id { get; } = "Markdown";
        public override string[] Extensions { get; } = { "md" };

        [ConvertMethod]
        public byte[] ToHTML(byte[] data)
        {
            string content = data.GetString();
            string html = Markdig.Markdown.ToHtml(content);
            return html.GetBytes();
        }

        [ConvertMethod]
        public byte[] ToText(byte[] data)
        {
            string content = data.GetString();
            string html = Markdig.Markdown.ToPlainText(content);
            return html.GetBytes();
        }
    }
}
