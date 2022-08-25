using MigratorLogParser.Constants;
using MigratorLogParser.Models;
using MigratorLogParser.Models.ProcessValidationIssues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Parsers.InvalidIdentity
{
    public class InvalidIdentityParser : ParserBase<Models.InvalidIdentity>
    {
        public InvalidIdentityParser() : base(Patterns.InvalidIdentity)
        {
        }

        public override bool ShouldParse(string text)
        {
            return IsMatch(text);
        }

        public override bool TryParse(string text, out Models.InvalidIdentity issue)
        {
            if (!IsMatch(text))
            {
                issue = default;
                return false;
            }

            var match = Match(text);

            issue = new Models.InvalidIdentity()
            {                
                Description = match.Groups["description"].Value,
                DisplayName = match.Groups["displayName"].Value,
                Property = match.Groups["property"].Value
            };

            return true;
        }
    }
}
