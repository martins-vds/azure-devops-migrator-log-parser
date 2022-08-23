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
    [Verb("data-migration", HelpText = "Parse the DataMigrationTool log")]
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
            var issues = _parser.ParseProcessValidationIssues(request.LogFilePath);

            var issuesByRef = issues.GroupBy(i => i.IssueRef);

            foreach (var group in issuesByRef)
            {
                var issueRef = group.Key;

                var issueType = group.First().GetType();

                _fileExporter.ExportToFile(group.Select(i => Convert.ChangeType(i, issueType)), $"{request.OutputDirectory}{Path.DirectorySeparatorChar}{issueRef}-{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            }

            return Unit.Task;
        }
    }
}
