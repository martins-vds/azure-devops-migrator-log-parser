namespace MigratorLogParser.Issues
{
    public class TF402584 : ProcessValidationIssue
    {
        public string? FielRuleAttributesNotSupported { get; set; }

        public TF402584()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402584-field-rule-attributes-for-or-not-arent-supported";
        }
    }
}