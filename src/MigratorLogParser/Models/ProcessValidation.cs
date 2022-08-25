using MigratorLogParser.Models.ProcessValidationIssues;

namespace MigratorLogParser.Models
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
