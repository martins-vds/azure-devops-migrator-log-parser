using MigratorLogParser.Models.ProcessValidationIssues;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class TF402574Parser : ProcessValidationIssueParser
    {
        public TF402574Parser()
            : base(@"(?<issueRef>TF\d+): ProcessConfiguration doesn't specify required TypeField (?<typeField>[^\.]+)\.")
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

            issue = new TF402574()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                TypeField = match.Groups["typeField"].Value
            };

            return true;
        }
    }
}