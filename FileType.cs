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
                .Where(m => m.GetCustomAttribute<ConvertMethodAttribute>() != null && m.Name.StartsWith("To") && m.Name.Length > 2 &&
                    m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(byte[]) && m.ReturnType == typeof(byte[]))
                .Select(m => m.Name[2..]).ToArray();
        }

        public bool IsConvertible(string id)
        {
            var method = GetType().GetMethod("To" + id);
            return method != null && method.GetCustomAttribute<ConvertMethodAttribute>() != null &&
                method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(byte[]) && method.ReturnType == typeof(byte[]);
        }

        public byte[] Convert(string id, byte[] data)
        {
            try
            {
                if (IsConvertible(id)) return (byte[]?)GetType().GetMethod("To" + id)?.Invoke(this, new[] { data }) ?? throw new ConversionException();
                else throw new ConversionException($"{Id}から{id}に変換することはできません");
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? new Exception();
            }
        }
    }
}
