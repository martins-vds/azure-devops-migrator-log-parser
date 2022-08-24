namespace MigratorLogParser.Issues
{
    public class TF402580 : ProcessValidationIssue
    {
        public string? WitName { get; set; }

        public TF402580()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402580-you-can-only-use-the-name-witname-for-a-single-work-item-type";
        }
    }
}