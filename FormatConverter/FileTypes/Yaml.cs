using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
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
        public string ToJSON(string data)
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
                            YamlValueType.Timestamp => JsonValue.Create(((YamlScalarNode)yamlNode).GetDateTimeStringValue()),
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
            using (var reader = new StringReader(data)) yaml.Load(reader);

            JsonNode json = new JsonArray();
            foreach (var doc in yaml.Documents)
            {
                ((JsonArray)json).Add(YamlNodeToJsonNode(doc.RootNode));
            }
            if (((JsonArray)json).Count == 1) json = ((JsonArray)json).ElementAt(0) ?? json;

            return json.ToJsonString(new JsonSerializerOptions() { WriteIndented = true });
        }

        [ConvertMethod(UseOutputFilePath = true)]
        public void ToXML(string data, string output)
        {
            XmlElement YamlNodeToXmlElem(YamlNode yamlNode, XmlElement baseElem)
            {
                switch (yamlNode.NodeType)
                {
                    case YamlNodeType.Scalar:
                        var type = ((YamlScalarNode)yamlNode).GetYamlValueType();
                        baseElem.InnerText = type switch
                        {
                            YamlValueType.Null => "null",
                            YamlValueType.Int => ((YamlScalarNode)yamlNode).GetInt64Value().ToString(),
                            YamlValueType.Float => ((YamlScalarNode)yamlNode).GetDecimalValue().ToString(),
                            YamlValueType.Inf => ((YamlScalarNode)yamlNode).Value.StartsWith("-") ? "-Infinity" : "Infinity",
                            YamlValueType.NaN => "NaN",
                            YamlValueType.Bool => ((YamlScalarNode)yamlNode).GetBooleanValue().ToString(),
                            YamlValueType.Str => ((YamlScalarNode)yamlNode).GetStringValue(),
                            YamlValueType.Timestamp => ((YamlScalarNode)yamlNode).GetDateTimeStringValue().ToString(),
                            YamlValueType.Others => ((YamlScalarNode)yamlNode).GetStringValue(),
                            _ => throw new ConversionException("不正な値があります")
                        };
                        break;
                    case YamlNodeType.Mapping:
                        foreach (var (key, value) in (YamlMappingNode)yamlNode)
                        {
                            string keyStr = key.GetString();
                            bool isMerge = true;
                            if (keyStr == "<<" && (value is YamlMappingNode || value is YamlSequenceNode))
                            {
                                void Merge(YamlMappingNode mapping)
                                {
                                    foreach (var (mergeKey, mergeValue) in mapping)
                                    {
                                        string mergeKeyStr = mergeKey.GetString();
                                        var mergeElem = baseElem.GetElementsByTagName(mergeKeyStr)?[0];
                                        if (mergeElem is not null) mergeElem.ParentNode?.RemoveChild(mergeElem);
                                        var elem = baseElem.OwnerDocument.CreateElement(XmlFunctions.ConvertToCorrectName(mergeKeyStr));
                                        baseElem.AppendChild(YamlNodeToXmlElem(mergeValue, elem));
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
                                    else isMerge = false;
                                }
                            }
                            else isMerge = false;
                            
                            if (!isMerge) 
                            {
                                var elem = baseElem.OwnerDocument.CreateElement(XmlFunctions.ConvertToCorrectName(keyStr));
                                baseElem.AppendChild(YamlNodeToXmlElem(value, elem));
                            }
                        }
                        break;
                    case YamlNodeType.Sequence:
                        foreach (var item in (YamlSequenceNode)yamlNode)
                        {
                            var elem = baseElem.OwnerDocument.CreateElement("item");
                            baseElem.AppendChild(YamlNodeToXmlElem(item, elem));
                        }
                        break;
                    default:
                        throw new ConversionException("不正な値があります");
                }

                return baseElem;
            }

            var yaml = new YamlStream();
            using (var reader = new StringReader(data)) yaml.Load(reader);
            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("root");
            xmlDoc.AppendChild(root);

            if (yaml.Documents.Count == 1)
            {
                YamlNodeToXmlElem(yaml.Documents[0].RootNode, root);
            }
            else
            {
                foreach (var doc in yaml.Documents)
                {
                    var docElem = xmlDoc.CreateElement("document");
                    root.AppendChild(docElem);
                    YamlNodeToXmlElem(doc.RootNode, docElem);
                }
            }

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
