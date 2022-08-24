using CommandLine;
using MediatR;
using MigratorLogParser.Exporters;
using MigratorLogParser.Parsers.DataMigrationTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Commands.ParseDataMigrationTool
{
    [Verb("data-migration-tool", Hidden = false, HelpText = "Parse the DataMigrationTool log")]
    public class ParseDataMigrationLogCommand : IRequest
    {
        [Option(shortName: 'f', longName: "file-path", Required = true, HelpText = "Path to the DataMigrationTool.log file")]
        public string? LogFilePath { get; set; }

        [Option(shortName: 'o', longName: "output", Required = true, HelpText = "Output directory for parsed files")]
        public string? OutputDirectory { get; set; }
    }

    public class ParseDataMigrationLogCommandHandler : IRequestHandler<ParseDataMigrationLogCommand>
    {
        private readonly DataMigrationToolLogParser _parser;
        private readonly IFileExporter _fileExporter;

        public ParseDataMigrationLogCommandHandler(DataMigrationToolLogParser parser, IFileExporter fileExporter)
        {
            _parser = parser;
            _fileExporter = fileExporter;
        }

        public Task<Unit> Handle(ParseDataMigrationLogCommand request, CancellationToken cancellationToken)
        {
            var dataMigrationToolLogParsed = _parser.Parse(request.LogFilePath);

            foreach (var projectValidationResult in dataMigrationToolLogParsed)
            {
                var issuesByRef = projectValidationResult.Issues.GroupBy(i => i.IssueRef);

                issuesByRef.AsParallel().ForAll(issues =>
                {
                    var issueRef = issues.Key;

                    var issueType = issues.First().GetType();

                    _fileExporter.ExportToFile(issues.Select(i => Convert.ChangeType(i, issueType)), $"{request.OutputDirectory}{Path.DirectorySeparatorChar}{projectValidationResult.ProjectName}-{issueRef}-{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
                });

                //foreach (var issues in issuesByRef)
                //{
                //    var issueRef = issues.Key;

                //    var issueType = issues.First().GetType();

                //    _fileExporter.ExportToFile(issues.Select(i => Convert.ChangeType(i, issueType)), $"{request.OutputDirectory}{Path.DirectorySeparatorChar}{projectValidationResult.ProjectName}-{issueRef}-{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
                //}

            }

            return Unit.Task;
        }
    }
}
