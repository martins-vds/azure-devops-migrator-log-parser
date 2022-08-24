using Microsoft.Extensions.FileSystemGlobbing.Internal;
using MigratorLogParser.Parsers.DataMigrationTool;

namespace MigratorLogParser.Issues
{
    public class ProcessValidationIssue
    {
        public string? File { get; set; } = string.Empty;
        public int? LineNumber { get; set; } = -1;
        public string? IssueRef { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Remediation { get; set; } = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops";
    }
}