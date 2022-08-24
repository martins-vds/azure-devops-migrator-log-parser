using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Issues
{
    public class TF401107 : ProcessValidationIssue
    {
        public string? ElementName { get; set; }
        public string? AttributeName { get; set; }
    }
}
