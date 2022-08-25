namespace MigratorLogParser.Models
{
    public class MigrationLog
    {
        public ICollection<MissingPermission> MissingPermissions { get; private set; } = new List<MissingPermission>();
        public ICollection<InvalidIdentity> InvalidIdentities { get; private set; } = new List<InvalidIdentity>();
        public ICollection<GlobalList> GlobalLists { get; private set; } = new HashSet<GlobalList>();
        public ICollection<ProcessValidation> ProcessValidationIssues { get; private set; } = new List<ProcessValidation>();
    }
}
