using MigratorLogParser.Issues;

namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class TF401107Parser : ProcessValidationIssueParser
    {
        public TF401107Parser() 
            : base(@"The following element contains an error: (?<elementName>.*(?=\.\s*TF)). (?<issueRef>TF\d+): The attribute '(?<attributeName>[^']+)' is missing from the element\.")
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

            issue = new TF401107()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                ElementName = match.Groups["elementName"].Value,
                AttributeName = match.Groups["attributeName"].Value
            };

            return true;
        }
    }
}