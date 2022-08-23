using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Exporters
{
    public class CsvExporter : IFileExporter
    {
        public void ExportToFile<T>(IEnumerable<T> records,string filePath)
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(records);
        }
    }
}
