namespace MigratorLogParser.Issues
{
    public class TF402583 : ProcessValidationIssue
    {
        public string? CustomLink { get; set; }
        public TF402583()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402583-custom-link-type-name-is-invalid-because-custom-link-types-arent-supported";
        }
    }
}