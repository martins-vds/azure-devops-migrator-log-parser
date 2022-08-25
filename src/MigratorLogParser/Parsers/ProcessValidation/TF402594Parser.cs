using MigratorLogParser.Models.ProcessValidationIssues;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class TF402594Parser : ProcessValidationIssueParser
    {
        public TF402594Parser()
            : base(@"(?<issueRef>TF\d+): File violates the schema with the following error: The element '(?<elementName>[^']+)' has incomplete content. List of possible elements expected: (?<expectedElements>[^\.]+)\.")
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

            issue = new TF402594()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                ElementName = match.Groups["elementName"].Value,
                ExpectedElements = match.Groups["expectedElements"].Value
            };

            return true;
        }
    }
}