using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

using FormatConverter.Functions;

namespace FormatConverter.FileTypes
{
    internal class Json : FileType
    {
        public override string Name { get; } = "JSON";
        public override string Id { get; } = "JSON";
        public override string[] Extensions { get; } = { "json" };

        [ConvertMethod]
        public byte[] ToYAML(byte[] data)
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

            var jsonData = JsonDocument.Parse(data.GetString());
            var writer = new StringWriter();
            var yaml = new YamlStream(new YamlDocument(JsonElemToYamlNode(jsonData.RootElement)));
            yaml.Save(writer, false);

            return writer.ToString().GetBytes();
        }
    }
}
