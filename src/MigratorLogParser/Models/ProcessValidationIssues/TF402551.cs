namespace MigratorLogParser.Models.ProcessValidationIssues
{
    public class TF402551 : ProcessValidationIssue
    {
        public string? WitName { get; set; }
        public string? ElementName { get; set; }
        public string? StateName { get; set; }

        public TF402551()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402551-work-item-type-witname-doesnt-define-workflow-state-statename-which-is-required-because-processconfiguration-maps-it-to-a-metastate-for-element-elementname";
        }
    }
}