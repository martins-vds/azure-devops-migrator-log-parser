namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class ProcessValidationIssue
    {
        public string? ProjectName { get; set; }
        public string? File { get; set; }
        public int? LineNumber { get; set; }
        public string? IssueRef { get; set; }
        public string? Description { get; set; }
        public string? Remediation { get; set; }
    }
}