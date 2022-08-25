using Microsoft.Extensions.Logging;
using MigratorLogParser.Models;
using MigratorLogParser.Parsers.GlobalList;
using MigratorLogParser.Parsers.InvalidIdentity;
using MigratorLogParser.Parsers.MissingPermission;
using MigratorLogParser.Parsers.ProcessValidation;
using System.IO.Abstractions;

namespace MigratorLogParser
{
    public enum ParsingState
    {
        None,
        MissingPermission,
        Identity,
        GlobalList,
        ProcessValidation
    }

    public class LogParser
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;

        private readonly InvalidIdentityParser _invalidIdentityParser = new InvalidIdentityParser();
        private readonly MissingPermissionParser _missingPermissionParser = new MissingPermissionParser();
        private readonly GlobalListParser _globalListParser = new GlobalListParser();
        private readonly ProcessValidationParser _processValidationParser = new ProcessValidationParser();
        private readonly IEnumerable<ProcessValidationIssueParser> _processValidationParsers = new List<ProcessValidationIssueParser>()
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

        public LogParser(IFileSystem fileSystem, ILogger<LogParser> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public MigrationLog Parse(string logFile)
        {
            var entries = _fileSystem.File.ReadLines(logFile);

            var migrationLog = new MigrationLog();

            ProcessValidation? currentProjectValidation = default;

            ParsingState _state;

            foreach (var entry in entries)
            {
                _state = ChangeState(entry);

                switch (_state)
                {
                    case ParsingState.ProcessValidation:
                        if (currentProjectValidation is null)
                        {
                            _processValidationParser.TryParse(entry, out currentProjectValidation);
                            migrationLog.ProcessValidationIssues.Add(currentProjectValidation);
                        }

                        if (_processValidationParser.IsIssue(entry))
                        {
                            var issueParsed = false;
                            foreach (var subParser in _processValidationParsers)
                            {
                                if (subParser.TryParse(entry, out var issue))
                                {
                                    currentProjectValidation?.Issues.Add(issue);
                                    issueParsed = true;
                                    break;
                                }
                            }

                            if (!issueParsed)
                            {
                                _logger.LogWarning("    Issue not parsed for project '{project}': '{entry}'", currentProjectValidation!.ProjectName, entry);
                            }
                        }

                        if (_processValidationParser.IsEndOfProjectIssues(entry))
                        {
                            var numberOfIssues = _processValidationParser.ParseNumberOfIssues(entry);

                            if (numberOfIssues != currentProjectValidation!.Issues.Count)
                            {
                                _logger.LogWarning("    {issuesNotParsed} issues were not parsed for project '{project}'.", numberOfIssues - currentProjectValidation.Issues.Count, currentProjectValidation.ProjectName);
                            }

                            currentProjectValidation = default;
                        }

                        break;
                    case ParsingState.MissingPermission:

                        if (_missingPermissionParser.TryParse(entry, out var missingPermission))
                        {
                            migrationLog.MissingPermissions.Add(missingPermission);
                        }

                        break;
                    case ParsingState.Identity:
                        if (_invalidIdentityParser.TryParse(entry, out var invalidIdentity))
                        {
                            migrationLog.InvalidIdentities.Add(invalidIdentity);
                        }
                        break;
                    case ParsingState.GlobalList:
                        if (_globalListParser.TryParse(entry, out var globalList))
                        {
                            migrationLog.GlobalLists.Add(globalList);
                        }
                        break;

                }
            }

            return migrationLog;
        }

        private ParsingState ChangeState(string entry)
        {
            if (_processValidationParser.ShouldParse(entry))
            {
                return ParsingState.ProcessValidation;
            }

            if (_missingPermissionParser.ShouldParse(entry))
            {
                return ParsingState.MissingPermission;
            }

            if (_invalidIdentityParser.ShouldParse(entry))
            {
                return ParsingState.Identity;
            }

            if (_globalListParser.ShouldParse(entry))
            {
                return ParsingState.GlobalList;
            }

            return ParsingState.None;
        }
    }
}