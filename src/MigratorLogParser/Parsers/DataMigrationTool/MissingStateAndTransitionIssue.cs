namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class MissingStateAndTransitionIssue : ProcessValidationIssue
    {
        public string? WitName { get; set; }
        public string? ElementName { get; set; }
        public string? StateName { get; set; }
    }
}