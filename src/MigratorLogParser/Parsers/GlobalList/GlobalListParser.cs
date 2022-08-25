using MigratorLogParser.Constants;
using MigratorLogParser.Models.ProcessValidationIssues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Parsers.GlobalList
{
    public class GlobalListParser : ParserBase<Models.GlobalList>
    {
        public GlobalListParser() : base(Patterns.GlobalList)
        {
        }

        public override bool ShouldParse(string text)
        {
            return IsMatch(text);
        }

        public override bool TryParse(string text, out Models.GlobalList issue)
        {
            if (!IsMatch(text))
            {
                issue = default;
                return false;
            }

            var match = Match(text);

            issue = new Models.GlobalList()
            {                
                Description = match.Groups["description"].Value,
                ListName = match.Groups["globalList"].Value,
            };

            return true;
        }
    }
}
