using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Models
{
    public class Issue
    {
        public string? Description { get; set; } = string.Empty;
        public string? IssueRef { get; set; } = string.Empty;
        public string? Remediation { get; set; } = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops";
    }
}
