using MigratorLogParser.Issues;

namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class TF400508Parser : ProcessValidationIssueParser
    {
        public TF400508Parser() 
            : base(@"The following element contains an error: (?<elementName>.*(?=\.\s*TF)). (?<issueRef>TF\d+): No columns are defined. You must define at least one column\.")
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

            issue = new TF400508()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                ElementName = match.Groups["elementName"].Value
            };

            return true;
        }
    }
}