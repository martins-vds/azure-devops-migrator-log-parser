using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Issues
{
    public class ProcessValidation
    {
        public string? ProjectName { get; set; }
        public IList<ProcessValidationIssue> Issues { get; set; } = new List<ProcessValidationIssue>();
        public ProcessValidation()
        {

        }
    }
}
