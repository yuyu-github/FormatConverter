using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    public static class FileTypeList
    {
        static FileType[] list = { };
        public static FileType[] List
        {
            get { return list; }
            set
            {
                list = value;
                UpdateDictionary();
            }
        }

        public static Dictionary<string, FileType> IdDictionary { get; private set; } = new();
        public static Dictionary<string, FileType> ExtensionDictionary { get; private set; } = new();

        static void UpdateDictionary()
        {
            IdDictionary = list.ToDictionary(t => t.Id, t => t);
            ExtensionDictionary = list.SelectMany(t => t.Extensions, (t, e) => new { e, t }).ToDictionary(d => d.e, d => d.t);
        }
    }
}
