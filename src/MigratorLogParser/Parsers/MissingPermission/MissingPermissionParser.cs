using MigratorLogParser.Constants;
using MigratorLogParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Parsers.MissingPermission
{
    public class MissingPermissionParser : ParserBase<Models.MissingPermission>
    {
        public MissingPermissionParser() : base(Patterns.MissingPermission)
        {
        }

        public override bool ShouldParse(string text)
        {
            return IsMatch(text);
        }

        public override bool TryParse(string text, out Models.MissingPermission issue)
        {
            if (!IsMatch(text))
            {
                issue = default;
                return false;
            }

            var match = Match(text);

            issue = new Models.MissingPermission()
            {
                Permission = match.Groups["permission"].Value,
                Group = match.Groups["group"].Value,
                Scope = match.Groups["scope"].Value,
                IssueRef = match.Groups["issueRef"].Value,
                Description = match.Groups["description"].Value,
            };

            return true;
        }
    }
}
