using CommandLine;
using MediatR;
using Microsoft.Extensions.Logging;
using MigratorLogParser.Exporters;
using System.IO.Abstractions;

namespace MigratorLogParser.Commands
{
    public class ParseLogCommand : IRequest
    {
        [Option(shortName: 'f', longName: "file-path", Required = true, HelpText = "Path to the DataMigrationTool.log file")]
        public string? LogFilePath { get; set; }

        [Option(shortName: 'o', longName: "output", Required = true, HelpText = "Output directory for parsed files")]
        public string? OutputDirectory { get; set; }
    }

    public class ParseLogCommandHandler : IRequestHandler<ParseLogCommand>
    {
        private readonly LogParser _parser;
        private readonly IFileExporter _fileExporter;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<ParseLogCommandHandler> _logger;

        public ParseLogCommandHandler(LogParser parser, IFileExporter fileExporter, IFileSystem fileSystem, ILogger<ParseLogCommandHandler> logger)
        {
            _parser = parser;
            _fileExporter = fileExporter;
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public Task<Unit> Handle(ParseLogCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Parsing issues...");

            var migrationLog = _parser.Parse(request.LogFilePath!);

            var now = DateTime.Now.ToString("yyyyMMddHHmmss");
            var outputPath = $"{request.OutputDirectory}{_fileSystem.Path.DirectorySeparatorChar}migration-issues-{now}";

            if (!_fileSystem.Directory.Exists(outputPath)) { _fileSystem.Directory.CreateDirectory(outputPath); }

            if (migrationLog.GlobalLists.Any())
            {
                _logger.LogInformation("Exporting {globalList} global lists...", migrationLog.GlobalLists.Count);
                _fileExporter.ExportToFile(migrationLog.GlobalLists, $"{outputPath}{_fileSystem.Path.DirectorySeparatorChar}global-lists.csv");
            }
            else
            {
                _logger.LogInformation("No global lists parsed.");
            }

            if (migrationLog.MissingPermissions.Any())
            {
                _logger.LogInformation("Exporting {missingPermissions} missing permission issues...", migrationLog.MissingPermissions.Count);
                _fileExporter.ExportToFile(migrationLog.MissingPermissions, $"{outputPath}{_fileSystem.Path.DirectorySeparatorChar}missing-permissions.csv");
            }
            else
            {
                _logger.LogInformation("No missing permission issues parsed.");
            }

            if (migrationLog.InvalidIdentities.Any())
            {
                _logger.LogInformation("Exporting {identities} identities with invalid characters...", migrationLog.InvalidIdentities.Count);
                _fileExporter.ExportToFile(migrationLog.InvalidIdentities, $"{outputPath}{_fileSystem.Path.DirectorySeparatorChar}identities-with-invalid-characters.csv");
            }
            else
            {
                _logger.LogInformation("No invalid identities parsed.");
            }

            if (migrationLog.ProcessValidationIssues.Any())
            {
                var processValidationIssuesFolder = $"{outputPath}{_fileSystem.Path.DirectorySeparatorChar}process-validation-issues";

                if (!_fileSystem.Directory.Exists(processValidationIssuesFolder)) { _fileSystem.Directory.CreateDirectory(processValidationIssuesFolder); }

                _logger.LogInformation("Exporting process validation issues...");
                foreach (var projectValidationResult in migrationLog.ProcessValidationIssues)
                {
                    var issuesByRef = projectValidationResult.Issues.GroupBy(i => i.IssueRef);

                    issuesByRef.AsParallel<IGrouping<string, Models.ProcessValidationIssues.ProcessValidationIssue>>().ForAll(issues =>
                    {
                        var issueRef = issues.Key;

                        var issueType = issues.First().GetType();

                        _fileExporter.ExportToFile(issues.Select(i => Convert.ChangeType(i, issueType)), $"{processValidationIssuesFolder}{_fileSystem.Path.DirectorySeparatorChar}{projectValidationResult.ProjectName}-{issueRef}.csv");
                    });
                }
            }
            else
            {
                _logger.LogInformation("No process validation issues parsed.");
            }

            _logger.LogInformation("Done.");

            return Unit.Task;
        }
    }
}
