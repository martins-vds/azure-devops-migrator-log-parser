namespace MigratorLogParser.Models.ProcessValidationIssues
{
    public class ProcessValidationIssue : Issue
    {
        public string? File { get; set; } = string.Empty;
        public int? LineNumber { get; set; } = -1;

        public ProcessValidationIssue()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops";
        }
    }
}