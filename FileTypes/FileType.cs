using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.FileTypes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ConvertMethodAttribute : Attribute { }

    public abstract class FileType
    {
        public abstract string Name { get; }
        public abstract string Id { get; }
        public abstract string[] Extensions { get; }

        public string[] GetConvertibleTypes()
        {
            return GetType().GetMethods()
                .Where(m => m.GetCustomAttribute<ConvertMethodAttribute>() != null && m.Name.StartsWith("To") && m.Name.Length > 2)
                .Select(m => m.Name[2..]).ToArray();
        }

        public bool IsConvertible(string id)
        {
            var method = GetType().GetMethod("To" + id);
            return method != null && method.GetCustomAttribute<ConvertMethodAttribute>() != null;
        }

        public byte[] Convert(string id, byte[] data)
        {
            if (IsConvertible(id)) return (byte[]?)GetType().GetMethod("To" + id)?.Invoke(this, new[] { data }) ?? new byte[0];
            else return new byte[0];
        }
    }
}
