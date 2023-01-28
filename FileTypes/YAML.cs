using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

using FormatConverter.Functions;
using FormatConverter.Functions.FileTypes;

namespace FormatConverter.FileTypes
{
    internal class Yaml : FileType
    {
        public override string Name { get; } = "YAML";
        public override string Id { get; } = "YAML";
        public override string[] Extensions { get; } = { "yml", "yaml" };

        [ConvertMethod]
        public byte[] ToJSON(byte[] data)
        {
            JsonNode? YamlNodeToJsonNode(YamlNode yamlNode)
            {
                switch (yamlNode.NodeType)
                {
                    case YamlNodeType.Scalar:
                        var type = ((YamlScalarNode)yamlNode).GetYamlValueType();
                        return type switch
                        {
                            YamlValueType.Null => null,
                            YamlValueType.Int => JsonValue.Create(((YamlScalarNode)yamlNode).GetInt64Value()),
                            YamlValueType.Float => JsonValue.Create(((YamlScalarNode)yamlNode).GetDecimalValue()),
                            YamlValueType.Inf => JsonValue.Create(((YamlScalarNode)yamlNode).Value.StartsWith("-") ? "-Infinity" : "Infinity"),
                            YamlValueType.NaN => JsonValue.Create("NaN"),
                            YamlValueType.Bool => JsonValue.Create(((YamlScalarNode)yamlNode).GetBooleanValue()),
                            YamlValueType.Str => JsonValue.Create(((YamlScalarNode)yamlNode).GetStringValue()),
                            YamlValueType.Timestamp => JsonValue.Create(((YamlScalarNode)yamlNode).GetDateTimeValue()),
                            YamlValueType.Others => JsonValue.Create(((YamlScalarNode)yamlNode).GetStringValue()),
                            _ => throw new ConversionException("不正な値があります")
                        };
                    case YamlNodeType.Mapping:
                        JsonObject objectNode = new JsonObject();
                        foreach (var (key, value) in (YamlMappingNode)yamlNode)
                        {
                            string keyStr = key.GetString();
                            if (keyStr == "<<" && (value is YamlMappingNode || value is YamlSequenceNode))
                            {
                                void Merge(YamlMappingNode mapping)
                                {
                                    foreach (var (mergeKey, mergeValue) in mapping)
                                    {
                                        string mergeKeyStr = mergeKey.GetString();
                                        if (objectNode.ContainsKey(mergeKeyStr)) objectNode[mergeKeyStr] = YamlNodeToJsonNode(mergeValue);
                                        else objectNode.Add(mergeKeyStr, YamlNodeToJsonNode(mergeValue));
                                    }
                                }

                                if (value is YamlMappingNode) Merge((YamlMappingNode)value);
                                else
                                {
                                    if (((YamlSequenceNode)value).Aggregate(true, (result, i) => result && (i is YamlMappingNode)))
                                    {
                                        foreach (var item in (YamlSequenceNode)value)
                                        {
                                            Merge((YamlMappingNode)item);
                                        }
                                    }
                                    else objectNode.Add(keyStr, YamlNodeToJsonNode(value));
                                }
                            } 
                            else objectNode.Add(keyStr, YamlNodeToJsonNode(value));
                        }
                        return objectNode;
                    case YamlNodeType.Sequence:
                        var arrayNode = new JsonArray();
                        foreach (var item in (YamlSequenceNode)yamlNode)
                        {
                            arrayNode.Add(YamlNodeToJsonNode(item));
                        }
                        return arrayNode;
                    default:
                        throw new ConversionException("不正な値があります");
                }
            }

            var yaml = new YamlStream();
            yaml.Load(new StringReader(data.GetString()));

            JsonNode json = new JsonArray();
            foreach (var doc in yaml.Documents)
            {
                ((JsonArray)json).Add(YamlNodeToJsonNode(doc.RootNode));
            }
            if (((JsonArray)json).Count == 1) json = ((JsonArray)json).ElementAt(0) ?? json;

            return json.ToJsonString(new JsonSerializerOptions() { WriteIndented = true }).GetBytes();
        }
    }
}
