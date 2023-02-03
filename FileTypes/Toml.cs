using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using Nett;

using FormatConverter.Functions;
using FormatConverter.Functions.FileTypes;

namespace FormatConverter.FileTypes
{
    internal class Toml : FileType
    {
        public override string Name { get; } = "TOML";
        public override string Id { get; } = "TOML";
        public override string[] Extensions { get; } = { "toml" };

        [ConvertMethod]
        public byte[] ToJSON(byte[] data)
        {
            JsonNode? TomlObjectToJsonNode(TomlObject tomlObject)
            {
                JsonNode? jsonNode = tomlObject.TomlType switch
                {
                    TomlObjectType.Bool => JsonValue.Create(tomlObject.Get<bool>()),
                    TomlObjectType.Int => JsonValue.Create(tomlObject.Get<long>()),
                    TomlObjectType.Float => JsonValue.Create(tomlObject.Get<double>()),
                    TomlObjectType.String => JsonValue.Create(tomlObject.Get<string>()),
                    TomlObjectType.DateTime => JsonValue.Create(tomlObject.Get<DateTimeOffset>().ToString()),
                    TomlObjectType.LocalDateTime => JsonValue.Create(tomlObject.Get<DateTime>().ToString()),
                    TomlObjectType.LocalTime => JsonValue.Create(tomlObject.Get<TimeSpan>().ToString()),
                    TomlObjectType.LocalDate => JsonValue.Create(DateOnly.FromDateTime(tomlObject.Get<DateTime>()).ToString()),
                    TomlObjectType.TimeSpan => JsonValue.Create(tomlObject.Get<TimeSpan>().ToString()),
                    TomlObjectType.Array => new JsonArray(),
                    TomlObjectType.Table => new JsonObject(),
                    TomlObjectType.ArrayOfTables => new JsonArray(),
                    _ => throw new ConversionException("不正な値があります")
                };

                if (tomlObject.TomlType == TomlObjectType.Table)
                {
                    foreach (var (key, value) in (TomlTable)tomlObject)
                    {
                        ((JsonObject?)jsonNode)?.Add(key, TomlObjectToJsonNode(value));
                    }
                }

                if (tomlObject.TomlType == TomlObjectType.Array)
                {
                    var tomlArray = (TomlArray)tomlObject;
                    for (var i = 0; i < tomlArray.Length; i++)
                    {
                        ((JsonArray?)jsonNode)?.Add(TomlObjectToJsonNode(tomlArray[i]));
                    }
                }

                if (tomlObject.TomlType == TomlObjectType.ArrayOfTables)
                {
                    var tomlArray = (TomlTableArray)tomlObject;
                    for (var i = 0; i < tomlArray.Count; i++)
                    {
                        ((JsonArray?)jsonNode)?.Add(TomlObjectToJsonNode(tomlArray[i]));
                    }
                }

                return jsonNode;
            }

            var toml = Nett.Toml.ReadString(data.GetString());
            var json = TomlObjectToJsonNode(toml);
            return (json?.ToJsonString(new JsonSerializerOptions() { WriteIndented = true }) ?? "").GetBytes();
        }

        [ConvertMethod]
        public byte[] ToYAML(byte[] data)
        {
            YamlNode TomlObjectToYamlNode(TomlObject tomlObject)
            {
                YamlNode yamlNode = tomlObject.TomlType switch
                {
                    TomlObjectType.Bool => new YamlScalarNode { Value = tomlObject.Get<bool>() ? "true" : "false" },
                    TomlObjectType.Int => new YamlScalarNode { Value = tomlObject.Get<long>().ToString() },
                    TomlObjectType.Float => new YamlScalarNode { Value = tomlObject.Get<double>().ToString() },
                    TomlObjectType.String => new YamlScalarNode { Value = tomlObject.Get<string>() },
                    TomlObjectType.DateTime => new YamlScalarNode { Value = tomlObject.Get<DateTimeOffset>().ToString() },
                    TomlObjectType.LocalDateTime => new YamlScalarNode { Value = tomlObject.Get<DateTime>().ToString() },
                    TomlObjectType.LocalTime => new YamlScalarNode { Value = tomlObject.Get<TimeSpan>().ToString() },
                    TomlObjectType.LocalDate => new YamlScalarNode { Value = DateOnly.FromDateTime(tomlObject.Get<DateTime>()).ToString() },
                    TomlObjectType.TimeSpan => new YamlScalarNode { Value = tomlObject.Get<TimeSpan>().ToString() },
                    TomlObjectType.Array => new YamlSequenceNode(),
                    TomlObjectType.Table => new YamlMappingNode(),
                    TomlObjectType.ArrayOfTables => new YamlSequenceNode(),
                    _ => throw new ConversionException("不正な値があります")
                };
                if (tomlObject.TomlType == TomlObjectType.String && ((YamlScalarNode)yamlNode).GetYamlValueType() != YamlValueType.Str)
                    ((YamlScalarNode)yamlNode).Style = ScalarStyle.DoubleQuoted;

                if (tomlObject.TomlType == TomlObjectType.Table)
                {
                    foreach (var (key, value) in (TomlTable)tomlObject)
                    {
                        ((YamlMappingNode)yamlNode).Add(key, TomlObjectToYamlNode(value));
                    }
                }

                if (tomlObject.TomlType == TomlObjectType.Array)
                {
                    var tomlArray = (TomlArray)tomlObject;
                    for (var i = 0; i < tomlArray.Length; i++)
                    {
                        ((YamlSequenceNode)yamlNode).Add(TomlObjectToYamlNode(tomlArray[i]));
                    }
                }

                if (tomlObject.TomlType == TomlObjectType.ArrayOfTables)
                {
                    var tomlArray = (TomlTableArray)tomlObject;
                    for (var i = 0; i < tomlArray.Count; i++)
                    {
                        ((YamlSequenceNode)yamlNode).Add(TomlObjectToYamlNode(tomlArray[i]));
                    }
                }

                return yamlNode;
            }

            var toml = Nett.Toml.ReadString(data.GetString());
            var yaml = new YamlStream(new YamlDocument(TomlObjectToYamlNode(toml)));
            var writer = new StringWriter();
            yaml.Save(writer, false);

            return writer.ToString().GetBytes();
        }
    }
}
