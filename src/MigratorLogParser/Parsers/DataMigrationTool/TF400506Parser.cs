using MigratorLogParser.Issues;

namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class TF400506Parser : ProcessValidationIssueParser
    {
        public TF400506Parser() 
            : base(@"The following element contains an error: (?<elementName>.*(?=\.\s*TF)). (?<issueRef>TF\d+): This element defines the states for work items that represent Bugs or Defects. Each state must exist in at least one of the work item types that are defined in: (?<elementCategory>[^\.]+). The following states do not exist in any of the work item types: (?<missingStates>[^\.$]+)\.")
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

            issue = new TF400506()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                ElementName = match.Groups["elementName"].Value,
                ElementCategory = match.Groups["elementCategory"].Value,
                MissingStates = match.Groups["missingStates"].Value
            };

            return true;
        }
    }
}