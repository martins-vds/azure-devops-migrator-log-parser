namespace MigratorLogParser.Exporters
{
    public interface IFileExporter
    {
        void ExportToFile<T>(IEnumerable<T> records, string filePath);
    }
}