namespace MigratorLogParser.Issues
{
    public class TF402574 : ProcessValidationIssue
    {
        public string? TypeField { get; set; }

        public TF402574()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402574-processconfiguration-doesnt-specify-required-typefield-typefield";
        }
    }
}