namespace MigratorLogParser.Issues
{
    public class TF402538 : ProcessValidationIssue
    {
        public string? RuleName { get; set; }

        public TF402538()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402538-field-rule-rulename-isnt-supported";
        }
    }
}