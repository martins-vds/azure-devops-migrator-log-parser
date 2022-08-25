namespace MigratorLogParser.Models.ProcessValidationIssues
{
    public class TF402581 : ProcessValidationIssue
    {
        public string? RefName { get; set; }
        public TF402581()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402581-you-can-only-use-the-refname-refname-for-a-single-work-item-type";
        }
    }
}