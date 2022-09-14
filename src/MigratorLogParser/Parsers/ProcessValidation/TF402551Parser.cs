using MigratorLogParser.Models.ProcessValidationIssues;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class TF402551Parser : ProcessValidationIssueParser
    {
        public TF402551Parser()
            : base(@"(?<issueRef>TF\d+): Work item type (?<witName>.*?(?=\s*doesn't)) doesn't define workflow state (?<stateName>[^,]+), which is required because ProcessConfiguration maps it to a metastate for element (?<elementName>.*?(?=\.$))\.")
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

            issue = new TF402551()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                WitName = match.Groups["witName"].Value,
                ElementName = match.Groups["elementName"].Value,
                StateName = match.Groups["stateName"].Value
            };

            return true;
        }
    }
}
