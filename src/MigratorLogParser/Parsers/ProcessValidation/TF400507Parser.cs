using MigratorLogParser.Models.ProcessValidationIssues;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class TF400507Parser : ProcessValidationIssueParser
    {
        public TF400507Parser()
            : base(@"The following element contains an error: (?<elementName>.*(?=\.\s*TF)). (?<issueRef>TF\d+): Each work item type must support an initial state value that matches one of the states defined in: (?<elementCategory>[^\.]+). The following work item types have initial states that do not include any states defined in the bug state configuration: (?<wit>[^\.$]*)\.")
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

            issue = new TF400507()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                ElementName = match.Groups["elementName"].Value,
                ElementCategory = match.Groups["elementCategory"].Value,
                Wit = match.Groups["wit"].Value
            };

            return true;
        }
    }
}