﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using YamlDotNet.Serialization;

using FormatConverter.Functions;

namespace FormatConverter.FileTypes
{
    internal class Csv : FileType
    {
        public override string Name { get; } = "CSV";
        public override string Id { get; } = "CSV";
        public override string[] Extensions { get; } = { "csv" };

        public DataTable LoadCSV(string data)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                AllowComments = true,
                MissingFieldFound = null,
            };

            using (var stream = new StringReader(data))
            using (var csv = new CsvReader(stream, config))
            using (var csvData = new CsvDataReader(csv))
            {
                var dataTable = new DataTable();
                try { dataTable.Load(csvData); } catch { throw new ConversionException("CSVの読み込みに失敗しました"); }
                return dataTable;
            }
        }

        [ConvertMethod]
        public byte[] ToJSON(byte[] data)
        {
            var dataTable = LoadCSV(data.GetString());

            var jsonData = new List<Dictionary<string, object?>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object?>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    object? value = null;
                    object item = row[column];
                    if (item is string)
                    {
                        string raw = (string)item;
                        if (long.TryParse(raw, out var longValue)) value = longValue;
                        else if (decimal.TryParse(raw, out var decimalValue)) value = decimalValue;
                        else if (raw == "") value = null;
                        else value = raw;
                    }
                    if (value is not null) dict.Add(column.ColumnName, value);
                }
                jsonData.Add(dict);
            }

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            return JsonSerializer.Serialize(jsonData, options).GetBytes();
        }

        [ConvertMethod]
        public byte[] ToYAML(byte[] data)
        {
            var dataTable = LoadCSV(data.GetString());

            var yamlData = new List<Dictionary<string, object?>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object?>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    object? value = null;
                    object item = row[column];
                    if (item is string)
                    {
                        string raw = (string)item;
                        if (long.TryParse(raw, out var longValue)) value = longValue;
                        else if (decimal.TryParse(raw, out var decimalValue)) value = decimalValue;
                        else if (raw == "") value = null;
                        else value = raw;
                    }
                    if (value is not null) dict.Add(column.ColumnName, value);
                }
                yamlData.Add(dict);
            }

            return new Serializer().Serialize(yamlData).GetBytes();
        }

        [ConvertMethod]
        public byte[] ToXML(byte[] data)
        {
            var dataTable = LoadCSV(data.GetString());

            var xml = new XmlDocument();
            var root = xml.CreateElement("root");
            xml.AppendChild(root);

            foreach (DataRow row in dataTable.Rows)
            {
                var itemElem = xml.CreateElement("item");
                foreach (DataColumn column in dataTable.Columns)
                {
                    string? value = null;
                    object item = row[column];
                    if (item is string)
                    {
                        value = (string)item;
                        if (value == "") value = null;
                    }
                    if (value is not null)
                    {
                        var valueElem = xml.CreateElement(column.ColumnName);
                        valueElem.InnerText = value;
                        itemElem.AppendChild(valueElem);
                    }
                }
                root.AppendChild(itemElem);
            }

            var stream = new MemoryStream();
            xml.Save(XmlWriter.Create(stream, new XmlWriterSettings()
            {
                Indent = true,
            }));
            stream.Position = 0;
            return new StreamReader(stream).ReadToEnd().GetBytes();
        }
    }
}
