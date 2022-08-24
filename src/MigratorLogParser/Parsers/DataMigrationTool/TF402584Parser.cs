using MigratorLogParser.Issues;

namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class TF402584Parser : ProcessValidationIssueParser
    {
        public TF402584Parser() 
            : base(@"(?<issueRef>TF\d+): Field rule attributes ""for"" or ""not"" aren't supported\.")
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

            issue = new TF402584()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                FielRuleAttributesNotSupported = "for; not"
            };

            return true;
        }
    }
}