using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Models.ProcessValidationIssues
{
    public class TF402547 : ProcessValidationIssue
    {
        public string? ElementName { get; set; }
        public string? WitName { get; set; }
        public string? Metastate { get; set; }

        public TF402547()
        {
            Remediation = "https://docs.microsoft.com/en-us/azure/devops/organizations/settings/work/import-process/resolve-errors?view=azure-devops#tf402547-element-elementname-requires-that-for-work-item-type-witname-you-map-at-least-one-state-to-metastate-metastatename";
        }
    }
}
