using MigratorLogParser.Issues;

namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class TF402596Parser : ProcessValidationIssueParser
    {
        public TF402596Parser() 
            : base(@"(?<issueRef>TF\d+): Category (?<category>[^\b]+) doesn't define work Item type (?<wit>[^\.$]+)\.")
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

            issue = new TF402596()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                Category = match.Groups["category"].Value,
                Wit = match.Groups["wit"].Value,
            };

            return true;
        }
    }
}