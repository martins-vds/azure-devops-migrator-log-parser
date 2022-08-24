using Microsoft.Extensions.Logging;
using MigratorLogParser.Issues;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class DataMigrationToolLogParser
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;

        private readonly ProcessValidationSectionParser processValidationSectionParser = new ProcessValidationSectionParser();
        private readonly IEnumerable<ProcessValidationIssueParser> _issueParsers = new List<ProcessValidationIssueParser>()
        {
            new TF402583Parser(),
            new TF402544Parser(),
            new TF402551Parser(),
            new TF400506Parser(),
            new TF400507Parser(),
            new TF400508Parser(),
            new TF400526Parser(),
            new TF401107Parser(),
            new TF402538Parser(),
            new TF402539Parser(),
            new TF402574Parser(),
            new TF402580Parser(),
            new TF402581Parser(),
            new TF402584Parser(),
            new TF402594Parser(),
            new TF402596Parser()
        };

        public DataMigrationToolLogParser(IFileSystem fileSystem, ILogger<DataMigrationToolLogParser> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public IEnumerable<ProcessValidation> Parse(string logFile)
        {
            var entries = _fileSystem.File.ReadLines(logFile);
            var issues = new List<ProcessValidation>();
            ProcessValidation projectValidation = default;

            var issuesFound = false;
            IDisposable? loginScope = default;

            foreach (var entry in entries)
            {
                if (issuesFound is false && processValidationSectionParser.TryParse(entry, out projectValidation))
                {
                    issues.Add(projectValidation);
                    issuesFound = true;
                    loginScope = _logger.BeginScope("Parsing issues for project {projectName}...", projectValidation.ProjectName);
                }

                if (issuesFound)
                {
                    if (processValidationSectionParser.IsIssue(entry))
                    {
                        var issueParsed = false;
                        foreach (var subParser in _issueParsers)
                        {
                            if (subParser.TryParse(entry, out var issue))
                            {
                                projectValidation?.Issues.Add(issue);
                                issueParsed = true;
                                break;
                            }
                        }

                        if (!issueParsed)
                        {
                            _logger.LogWarning("Issue not parsed: '{entry}'", entry);
                        }
                    }

                    if (processValidationSectionParser.IsEndOfProjectIssues(entry, projectValidation.ProjectName))
                    {
                        var numberOfIssues = processValidationSectionParser.ParseNumberOfIssues(entry, projectValidation.ProjectName);

                        if (numberOfIssues != projectValidation.Issues.Count)
                        {
                            _logger.LogWarning("{issuesNotParsed} issues were not parsed.", numberOfIssues - projectValidation.Issues.Count);
                        }

                        issuesFound = false;
                        loginScope?.Dispose();
                    }
                }
            }

            return issues;
        }
    }
}