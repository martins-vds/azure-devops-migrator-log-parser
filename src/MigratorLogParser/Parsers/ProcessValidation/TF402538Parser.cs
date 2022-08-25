using MigratorLogParser.Models.ProcessValidationIssues;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class TF402538Parser : ProcessValidationIssueParser
    {
        public TF402538Parser()
            : base(@"(?<issueRef>TF\d+): Field rule (?<ruleName>.*(?=\s*is)) isn't supported\.")
        {
        }

        public override bool TryParse(string text, out ProcessValidationIssue issue)
        {
            if (!IsMatch(text))
            {
                issue = default;
                return false;
            }

            var match = Match(text);

            issue = new TF402538()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                RuleName = match.Groups["ruleName"].Value
            };

            return true;
        }
    }
}