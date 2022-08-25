using MigratorLogParser.Models.ProcessValidationIssues;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class TF402580Parser : ProcessValidationIssueParser
    {
        public TF402580Parser()
            : base(@"(?<issueRef>TF\d+): You can only use the name (?<witName>.*(?=\s*for)) for a single work item type\.")
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

            issue = new TF402580()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                WitName = match.Groups["witName"].Value
            };

            return true;
        }
    }
}