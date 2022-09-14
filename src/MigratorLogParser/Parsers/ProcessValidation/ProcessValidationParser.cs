using MigratorLogParser.Constants;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class ProcessValidationParser : ParserBase<Models.ProcessValidation>
    {
        private const string StartOfProcessValidation = @$"{Patterns.ProcessValidation} - Starting validation of project \d+=(?<projectName>[^,]+),";
        private const string EndOfIssuesPattern = @$"{Patterns.ProcessValidation} - Validation failed for project (?<projectName>.*?(?=\s*with)) with (?<numberOfIssues>\d+) errors";
        private const string SuccessfullValidationPattern = @$"{Patterns.ProcessValidation} - Successfully validated project (?<projectName>.*)";

        public ProcessValidationParser() : base(Patterns.ProcessValidation)
        {
        }

        public override bool TryParse(string text, out Models.ProcessValidation issue)
        {
            if (!IsMatch(text, StartOfProcessValidation))
            {
                issue = default;
                return false;
            }

            var match = Match(text, StartOfProcessValidation);

            issue = new Models.ProcessValidation()
            {
                ProjectName = match.Groups["projectName"].Value
            };

            return true;
        }

        public override bool ShouldParse(string text)
        {
            if(IsMatch(text, Patterns.ProcessValidation) || IsIssue(text) || IsEndOfProcessValidation(text)) { return true; }

            return false;
        }

        public bool IsIssue(string text)
        {
            return IsMatch(text, Patterns.ProcessValidationIssue);
        }

        public bool IsEndOfProcessValidation(string text)
        {
            return IsMatch(text, EndOfIssuesPattern) || IsMatch(text, SuccessfullValidationPattern);
        }

        public int? ParseNumberOfIssues(string text)
        {
            var match = Match(text, EndOfIssuesPattern);
            var value = match.Groups["numberOfIssues"].Value;

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return int.Parse(value);
        }
    }
}