using MigratorLogParser.Models.ProcessValidationIssues;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class TF402539Parser : ProcessValidationIssueParser
    {
        public TF402539Parser()
            : base(@"(?<issueRef>TF\d+): Field (?<fieldName>[^\b]+) only allows the following field rules: (?<allowedRules>[^\.$]*)\.")
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

            issue = new TF402539()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                FieldName = match.Groups["fieldName"].Value,
                AllowedRules = match.Groups["allowedRules"].Value,
            };

            return true;
        }
    }
}