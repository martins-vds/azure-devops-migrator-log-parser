using MigratorLogParser.Parsers.DataMigrationTool;

namespace MigratorLogParser.Issues
{
    public class TF402544 : ProcessValidationIssue
    {
        public string? RefName { get; set; }
        public string? WitName { get; set; }
        public string? ElementName { get; set; }
        public TF402544()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402544-field-refname-defined-in-work-item-type-witname-requires-an-allowedvalues-rule-that-contains-values-to-support-element-elementname-specified-in-processconfiguration";
        }
    }
}