using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

using FormatConverter.Functions;
using FormatConverter.Functions.FileTypes;

namespace FormatConverter.FileTypes
{
    internal class Json : FileType
    {
        public override string Name { get; } = "JSON";
        public override string Id { get; } = "JSON";
        public override string[] Extensions { get; } = { "json" };

        [ConvertMethod(UseOutputFilePath = true)]
        public void ToYAML(string data, string output)
        {
            YamlNode JsonElemToYamlNode(JsonElement jsonElem)
            {
                YamlNode yamlNode = jsonElem.ValueKind switch
                {
                    JsonValueKind.Undefined => new YamlScalarNode() { Value = "null" },
                    JsonValueKind.Null => new YamlScalarNode() { Value = "null" },
                    JsonValueKind.String => new YamlScalarNode() { Value = jsonElem.GetString() },
                    JsonValueKind.Number => new YamlScalarNode() { Value = jsonElem.GetRawText() },
                    JsonValueKind.True => new YamlScalarNode() { Value = "true" },
                    JsonValueKind.False => new YamlScalarNode() { Value = "false" },
                    JsonValueKind.Object => new YamlMappingNode(),
                    JsonValueKind.Array => new YamlSequenceNode(),
                    _ => throw new ConversionException("不正な値があります")
                };
                if (jsonElem.ValueKind == JsonValueKind.String && ((YamlScalarNode)yamlNode).GetYamlValueType() != YamlValueType.Str)
                    ((YamlScalarNode)yamlNode).Style = ScalarStyle.DoubleQuoted;

                if (jsonElem.ValueKind == JsonValueKind.Object)
                {
                    foreach (var item in jsonElem.EnumerateObject())
                    {
                        var valueNode = JsonElemToYamlNode(item.Value);
                        ((YamlMappingNode)yamlNode).Add(item.Name, valueNode);
                    }
                }

                if (jsonElem.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in jsonElem.EnumerateArray())
                    {
                        var valueNode = JsonElemToYamlNode(item);
                        ((YamlSequenceNode)yamlNode).Add(valueNode);
                    }
                }

                return yamlNode;
            }

            var jsonData = JsonDocument.Parse(data);
            var yaml = new YamlStream(new YamlDocument(JsonElemToYamlNode(jsonData.RootElement)));
            using (var writer = new StreamWriter(output))
            {
                yaml.Save(writer, false);
            }
        }

        [ConvertMethod(UseOutputFilePath = true)]
        public void ToXML(string data, string output)
        {
            XmlElement JsonElemToXmlElem(JsonElement jsonElem, XmlElement baseElem)
            {
                switch (jsonElem.ValueKind)
                {
                    case JsonValueKind.Object:
                        foreach (var item in jsonElem.EnumerateObject())
                        {
                            var elem = baseElem.OwnerDocument.CreateElement(XmlFunctions.ConvertToCorrectName(item.Name));
                            baseElem.AppendChild(JsonElemToXmlElem(item.Value, elem));
                        }
                        break;
                    case JsonValueKind.Array:
                        foreach (var item in jsonElem.EnumerateArray())
                        {
                            var elem = baseElem.OwnerDocument.CreateElement("item");
                            baseElem.AppendChild(JsonElemToXmlElem(item, elem));
                        }
                        break;
                    case JsonValueKind.String:
                        baseElem.InnerText = jsonElem.GetString() ?? "";
                        break;
                    default:
                        baseElem.InnerText = jsonElem.GetRawText();
                        break;
                }

                return baseElem;
            }

            var jsonData = JsonDocument.Parse(data);
            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("root");
            xmlDoc.AppendChild(root);
            JsonElemToXmlElem(jsonData.RootElement, root);

            using (var writer = XmlWriter.Create(output, new XmlWriterSettings()
            {
                Indent = true,
            }))
            {
                xmlDoc.Save(writer);
            }
        }
    }
}
