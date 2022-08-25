namespace MigratorLogParser.Models.ProcessValidationIssues
{
    public class TF402539 : ProcessValidationIssue
    {
        public string? FieldName { get; set; }
        public string? AllowedRules { get; set; }
        public TF402539()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402539-field-refname-only-allows-the-following-rules-rulenames";
        }
    }
}