using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Issues
{
    public class TF400506 : ProcessValidationIssue
    {
        public string? ElementName { get; set; }
        public string? ElementCategory { get; set; }
        public string? MissingStates { get; set; }

        public TF400506()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf400506-this-element-defines-the-states-for-work-items-that-represent-bugs-or-defects";
        }
    }
}
