namespace MigratorLogParser.Models.ProcessValidationIssues
{
    public class TF402596 : ProcessValidationIssue
    {
        public string? Category { get; set; }
        public string? Wit { get; set; }

        public TF402596()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402596-category-categoryname-doesnt-define-work-item-type-witname";
        }
    }
}