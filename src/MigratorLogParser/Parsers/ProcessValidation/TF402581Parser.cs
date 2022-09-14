using MigratorLogParser.Models.ProcessValidationIssues;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class TF402581Parser : ProcessValidationIssueParser
    {
        public TF402581Parser()
            : base(@"(?<issueRef>TF\d+): You can only use the refname (?<refName>.*?(?=\s*for)) for a single work item type\.")
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

            issue = new TF402581()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                RefName = match.Groups["refName"].Value
            };

            return true;
        }
    }
}