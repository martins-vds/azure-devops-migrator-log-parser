using MigratorLogParser.Constants;
using MigratorLogParser.Models.ProcessValidationIssues;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public abstract class ProcessValidationIssueParser : ParserBase<ProcessValidationIssue>
    {
        protected ProcessValidationIssueParser(string pattern)
            : base($"{Patterns.ProcessValidationIssue}\\s*(?<description>{pattern})")
        {
        }
    }
}