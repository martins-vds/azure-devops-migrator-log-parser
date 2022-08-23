namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class MissingAllowedValuesIssue : ProcessValidationIssue
    {
        public string? RefName { get; set; }
        public string? WitName { get; set; }
        public string? ElementName { get; set; }
    }
}