using MigratorLogParser.Issues;

namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class TF402544Parser : ProcessValidationIssueParser
    {
        public TF402544Parser() 
            : base(@"(?<issueRef>TF\d+): Field (?<refName>.*(?=,\s+defined)), defined in work item type (?<witName>[^,]*), requires an ALLOWEDVALUES rule that contains values to support element (?<elementName>\S*) specified in ProcessConfiguration\.")
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

            issue = new TF402544()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,                
                RefName = match.Groups["refName"].Value,
                WitName = match.Groups["witName"].Value,
                ElementName = match.Groups["elementName"].Value
            };

            return true;
        }
    }
}