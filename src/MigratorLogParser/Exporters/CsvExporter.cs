using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO.Abstractions;
using System.Text;

namespace MigratorLogParser.Exporters
{
    public class CsvExporter : IFileExporter
    {
        public void ExportToFile<T>(IEnumerable<T> records, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Encoding = Encoding.UTF8 });

            csv.WriteRecords(records);
        }
    }
}
