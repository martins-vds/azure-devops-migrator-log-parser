using MigratorLogParser.Constants;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class ProcessValidationParser : ParserBase<Models.ProcessValidation>
    {
        private const string EndOfIssuesPattern = @"\[Info[^\]]+\] Step : ProcessValidation INFO - Validation failed for project (?<projectName>.*?(?=\s*with)) with (?<numberOfIssues>\d+) errors";

        public ProcessValidationParser() : base(Patterns.ProcessValidation)
        {
        }

        public override bool TryParse(string text, out Models.ProcessValidation issue)
        {
            if (!IsMatch(text))
            {
                issue = default;
                return false;
            }

            var match = Match(text);

            issue = new Models.ProcessValidation()
            {
                ProjectName = match.Groups["projectName"].Value
            };

            return true;
        }

        public override bool ShouldParse(string text)
        {
            if(IsMatch(text, Patterns.ProcessValidation) || IsIssue(text) || IsEndOfProjectIssues(text)) { return true; }

            return false;
        }

        public bool IsIssue(string text)
        {
            return IsMatch(text, Patterns.ProcessValidationIssue);
        }

        public bool IsEndOfProjectIssues(string text)
        {
            return IsMatch(text, EndOfIssuesPattern);
        }

        public int ParseNumberOfIssues(string text)
        {
            var match = Match(text, EndOfIssuesPattern);
            var value = match.Groups["numberOfIssues"].Value;

            if (string.IsNullOrWhiteSpace(value))
            {
                return -1;
            }

            return int.Parse(value);
        }
    }
}