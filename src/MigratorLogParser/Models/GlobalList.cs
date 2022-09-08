using System.Diagnostics.CodeAnalysis;

namespace MigratorLogParser.Models
{
    public sealed class GlobalList : Issue, IEquatable<GlobalList>
    {
        public string? ListName { get; set; }

        public GlobalList()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/reference/witadmin/manage-global-lists-for-work-item-types?view=azure-devops";
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as GlobalList);
        }

        public bool Equals(GlobalList? other)
        {
            return other is not null &&
                   ListName == other.ListName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ListName);
        }

        public static bool operator ==(GlobalList? left, GlobalList? right)
        {
            return EqualityComparer<GlobalList>.Default.Equals(left, right);
        }

        public static bool operator !=(GlobalList? left, GlobalList? right)
        {
            return !(left == right);
        }
    }
}