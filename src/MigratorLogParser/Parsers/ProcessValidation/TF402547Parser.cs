using MigratorLogParser.Models.ProcessValidationIssues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Parsers.ProcessValidation
{
    public class TF402547Parser : ProcessValidationIssueParser
    {
        public TF402547Parser()
            : base(@"(?<issueRef>TF\d+): Element (?<elementName>[\S]+) requires that for work item type (?<wit>[\S]+) you map at least one state to metastate (?<metastate>[^.]*).")
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

            issue = new TF402547()
            {
                File = match.Groups["file"].Value,
                LineNumber = ParseInt(match.Groups["lineNumber"].Value),
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
                ElementName = match.Groups["elementName"].Value,
                WitName = match.Groups["wit"].Value,
                Metastate = match.Groups["metastate"].Value,
            };

            return true;
        }
    }
}
