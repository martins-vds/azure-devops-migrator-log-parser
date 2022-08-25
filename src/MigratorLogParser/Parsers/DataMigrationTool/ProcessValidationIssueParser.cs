using MigratorLogParser.Issues;

namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public abstract class ProcessValidationIssueParser : ParserBase<ProcessValidationIssue>
    {
        private const string _preamble = @"\[Error[^\]]+\] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: (?<file>[^:]*):(?<lineNumber>\d*):";

        protected ProcessValidationIssueParser(string pattern)
            : base($"{_preamble}\\s*(?<description>{pattern})")
        {
        }
    }
}