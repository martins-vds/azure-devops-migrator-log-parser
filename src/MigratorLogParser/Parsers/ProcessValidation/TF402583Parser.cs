using MigratorLogParser.Models.ProcessValidationIssues;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class TF402583Parser : ProcessValidationIssueParser
    {
        public TF402583Parser() : base(@"(?<issueRef>TF\d+): Custom link type (?<customLink>\S+) is invalid because custom link types aren't supported\.")
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

            issue = new TF402583()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                CustomLink = match.Groups["customLink"].Value,
            };

            return true;
        }
    }


}