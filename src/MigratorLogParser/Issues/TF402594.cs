namespace MigratorLogParser.Issues
{
    public class TF402594 : ProcessValidationIssue
    {
        public string? ElementName { get; set; }
        public string? ExpectedElements { get; set; }

        public TF402594()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402594-file-violates-the-schema-with-the-following-error-error-message";
        }
    }
}