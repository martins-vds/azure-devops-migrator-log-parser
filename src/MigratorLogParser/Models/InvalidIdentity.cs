namespace MigratorLogParser.Models
{
    public class InvalidIdentity : Issue
    {
        public string? DisplayName { get; set; }
        public string? Property { get; set; }

        public InvalidIdentity()
        {
            Remediation = "Cleanup invalid characters from the identity object in your AD";
        }
    }
}