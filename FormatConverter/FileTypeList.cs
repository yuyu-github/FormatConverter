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
        public static Dictionary<string, List<FileType>> ExtensionDictionary { get; private set; } = new();

        static void UpdateDictionary()
        {
            IdDictionary = list.ToDictionary(t => t.Id, t => t);
            
            foreach (var type in list)
                foreach (var ext in type.Extensions)
                {
                    if (ExtensionDictionary.ContainsKey(ext)) ExtensionDictionary[ext].Add(type);
                    else ExtensionDictionary.Add(ext, new List<FileType> { type });
                }
        }
    }
}
