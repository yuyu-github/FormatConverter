using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FormatConverter.FileTypes
{
    public abstract class FileType
    {
        public abstract string Name { get; }
        public abstract string Id { get; }
        public abstract string[] Extensions { get; }

        public bool IsConvertMethod(MethodInfo method)
        {
            var attr = method.GetCustomAttribute<ConvertMethodAttribute>();
            return attr is not null &&
                method.Name.StartsWith("To") && method.Name.Length > 2 &&
                method.GetParameters().Length >= 1 &&

                (attr.UseInputFilePath ?
                method.GetParameters()[0].ParameterType == typeof(string) :
                method.GetParameters()[0].ParameterType == typeof(byte[]) || method.GetParameters()[0].ParameterType == typeof(string)) &&

                (attr.UseOutputFilePath ?
                method.GetParameters().Length >= 2 && method.GetParameters()[1].ParameterType == typeof(string) :
                method.ReturnType == typeof(byte[]) || method.ReturnType == typeof(string));
        }

        public string[] GetConvertibleTypes()
        {
            return GetType().GetMethods()
                .Where(IsConvertMethod)
                .Select(m => m.Name[2..])
                .OrderBy(n => n).ToArray();
        }

        public bool IsConvertible(string id)
        {
            var method = GetType().GetMethod("To" + id);
            return method is not null && IsConvertMethod(method);
        }

        public void Convert(string id, string input, string output)
        {
            try
            {
                if (IsConvertible(id))
                {
                    var method = GetType().GetMethod("To" + id);
                    var attr = method?.GetCustomAttribute<ConvertMethodAttribute>();
                    if (method is null || attr is null) throw new ConversionException();

                    var paramType = method.GetParameters()[0].ParameterType;
                    List<object?> args = new();
                    args.Add(attr.UseInputFilePath ? input :
                         paramType == typeof(byte[]) ? File.ReadAllBytes(input) : paramType == typeof(string) ? File.ReadAllText(input) : throw new ConversionException());
                    if (attr.UseOutputFilePath)
                    {
                        args.Add(output);
                        method.Invoke(this, args.ToArray());
                    }
                    else
                    {
                        var outputData = method.Invoke(this, args.ToArray());
                        if (method.ReturnType == typeof(byte[])) File.WriteAllBytes(output, (byte[]?)outputData ?? throw new ConversionException());
                        else if (method.ReturnType == typeof(string)) File.WriteAllText(output, (string?)outputData ?? throw new ConversionException());
                        else throw new ConversionException();
                    }
                }
                else throw new ConversionException($"{Id}から{id}に変換することはできません");
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Throw(e.InnerException ?? new());
                throw;
            }
        }
    }
}
