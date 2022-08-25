using MigratorLogParser.Issues;

namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class ProcessValidationSectionParser : ParserBase<ProcessValidation>
    {
        private const string EndOfIssuesPattern = @"\[Info[^\]]+\] Step : ProcessValidation INFO - Validation failed for project {0} with (?<numberOfIssues>\d+) errors";

        public ProcessValidationSectionParser() : base(@"\[Info[^\]]+\] Step : ProcessValidation INFO - Starting validation of project \d+=(?<projectName>[^,]+),")
        {
        }

        public override bool TryParse(string text, out ProcessValidation issue)
        {
            if (!IsMatch(text))
            {
                issue = default;
                return false;
            }

            var match = Match(text);

            issue = new ProcessValidation()
            {
                ProjectName = match.Groups["projectName"].Value
            };

            return true;
        }

        public bool IsIssue(string text)
        {
            return IsMatch(text, @"\[Error[^\]]+\] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: (?<file>[^:]*):(?<lineNumber>\d*)");
        }

        public bool IsEndOfProjectIssues(string text, string project)
        {
            return IsMatch(text, string.Format(EndOfIssuesPattern, project));
        }

        public int ParseNumberOfIssues(string text, string project)
        {
            var match = Match(text, string.Format(EndOfIssuesPattern, project));
            var value = match.Groups["numberOfIssues"].Value;

            if (string.IsNullOrWhiteSpace(value))
            {
                return -1;
            }

            return int.Parse(value);
        }
    }
}