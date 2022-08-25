namespace MigratorLogParser.Models
{
    public class MissingPermission : Issue
    {
        public string? Permission { get; set; }
        public string? Group { get; set; }
        public string? Scope { get; set; }

        public MissingPermission()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/migrate/migration-troubleshooting?view=azure-devops#isverror-100014";
        }
    }
}