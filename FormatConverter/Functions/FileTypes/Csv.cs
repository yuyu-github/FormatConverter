using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Data;
using CsvHelper;
using CsvHelper.Configuration;

namespace FormatConverter.Functions.FileTypes
{
    internal static class CsvFunctions
    {
        public static DataTable Load(string data)
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
    }
}
