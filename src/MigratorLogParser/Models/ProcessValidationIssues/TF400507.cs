namespace MigratorLogParser.Models.ProcessValidationIssues
{
    public class TF400507 : ProcessValidationIssue
    {
        public string? ElementName { get; set; }
        public string? ElementCategory { get; set; }
        public string? Wit { get; set; }

        public TF400507()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf400507-each-work-item-type-must-support-an-initial-state-value-that-matches-one-of-the-states-defined-in-bugworkitems";
        }
    }
}
