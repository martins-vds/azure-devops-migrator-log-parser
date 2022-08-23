using Microsoft.Extensions.Logging;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace MigratorLogParser.Parsers.DataMigrationTool
{
    public class DataMigrationToolLogParser
    {
        private const string StartOfIssuesPattern = @"\[Info\] Step : ProcessValidation INFO - Starting validation of project \d+=(?<projectName>[^,]+),";
        private const string EndOfIssuesPattern = @"\[Info\] Step : ProcessValidation INFO - Validation failed for project {0} with (?<numberOfIssues>\d+) errors";
        private const string ProcessValidationFailurePattern = @"\[Error\] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: (?<file>[^:]+):(?<lineNumber>\d*)";
        private const string CustomLinkPattern = @"(?<description>(?<issueRef>TF\d+): Custom link type (?<customLink>\S+) is invalid because custom link types aren't supported\.)";
        private const string MissingAllowedValuesPattern = @"(?<description>(?<issueRef>TF\d+): Field (?<refName>.*(?=,\s+defined)), defined in work item type (?<witName>[^,]*), requires an ALLOWEDVALUES rule that contains values to support element (?<elementName>\S*) specified in ProcessConfiguration\.)";
        private const string MissingStateAndTransitionPattern = @"(?<description>(?<issueRef>TF\d+): Work item type (?<witName>.*(?=\s*doesn't)) doesn't define workflow state (?<stateName>[^,]+), which is required because ProcessConfiguration maps it to a metastate for element (?<elementName>.*(?=\.$))\.)";
        private const string BugsMissingStatesPattern = @"(?<description>The following element contains an error: (?<elementName>.*(?=\.\s+TF)). TF400506: This element defines the states for work items that represent Bugs or Defects. Each state must exist in at least one of the work item types that are defined in: BugWorkItems. The following states do not exist in any of the work item types: (?<missingStates>.*)\.)";

        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;

        public DataMigrationToolLogParser(IFileSystem fileSystem, ILogger<DataMigrationToolLogParser> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public IEnumerable<ProcessValidationIssue> ParseProcessValidationIssues(string log)
        {
            var entries = _fileSystem.File.ReadLines(log);
            var issues = new List<ProcessValidationIssue>();

            var issuesFound = false;
            var projectName = string.Empty;
            var issuesParsed = 0;

            foreach (var entry in entries)
            {

                if (issuesFound is false && Regex.IsMatch(entry, StartOfIssuesPattern))
                {
                    var match = Regex.Match(entry, StartOfIssuesPattern);
                    projectName = match.Groups["projectName"].Value;
                    issuesFound = true;
                }

                using (_logger.BeginScope("Parsing issues for project {projectName}...", projectName))
                {

                    if (issuesFound && Regex.IsMatch(entry, ProcessValidationFailurePattern))
                    {
                        var issueLocationMatch = Regex.Match(entry, ProcessValidationFailurePattern);

                        if (Regex.IsMatch(entry, CustomLinkPattern))
                        {
                            var customLinkIssueMatch = Regex.Match(entry, CustomLinkPattern);

                            issues.Add(new CustomLinkIssue()
                            {
                                ProjectName = projectName,
                                File = issueLocationMatch.Groups["file"].Value,
                                LineNumber = int.Parse(string.IsNullOrWhiteSpace(issueLocationMatch.Groups["lineNumber"].Value) ? "-1" : issueLocationMatch.Groups["lineNumber"].Value),
                                IssueRef = customLinkIssueMatch.Groups["issueRef"].Value,
                                Description = customLinkIssueMatch.Groups["description"].Value,
                                CustomLink = customLinkIssueMatch.Groups["customLink"].Value,
                                Remediation = string.Empty
                            });
                            issuesParsed++;
                        }
                        else if (Regex.IsMatch(entry, MissingAllowedValuesPattern))
                        {
                            var missingAllowedValuesIssueMatch = Regex.Match(entry, MissingAllowedValuesPattern);

                            issues.Add(new MissingAllowedValuesIssue()
                            {
                                ProjectName = projectName,
                                File = issueLocationMatch.Groups["file"].Value,
                                LineNumber = int.Parse(string.IsNullOrWhiteSpace(issueLocationMatch.Groups["lineNumber"].Value) ? "-1" : issueLocationMatch.Groups["lineNumber"].Value),
                                IssueRef = missingAllowedValuesIssueMatch.Groups["issueRef"].Value,
                                Description = missingAllowedValuesIssueMatch.Groups["description"].Value,
                                Remediation = string.Empty,
                                RefName = missingAllowedValuesIssueMatch.Groups["refName"].Value,
                                WitName = missingAllowedValuesIssueMatch.Groups["witName"].Value,
                                ElementName = missingAllowedValuesIssueMatch.Groups["elementName"].Value
                            });
                            issuesParsed++;
                        }
                        else if (Regex.IsMatch(entry, MissingStateAndTransitionPattern))
                        {
                            var missingStateAndTransitionIssueMatch = Regex.Match(entry, MissingStateAndTransitionPattern);

                            issues.Add(new MissingStateAndTransitionIssue()
                            {
                                ProjectName = projectName,
                                File = issueLocationMatch.Groups["file"].Value,
                                LineNumber = int.Parse(string.IsNullOrWhiteSpace(issueLocationMatch.Groups["lineNumber"].Value) ? "-1" : issueLocationMatch.Groups["lineNumber"].Value),
                                IssueRef = missingStateAndTransitionIssueMatch.Groups["issueRef"].Value,
                                Description = missingStateAndTransitionIssueMatch.Groups["description"].Value,
                                Remediation = string.Empty,
                                WitName = missingStateAndTransitionIssueMatch.Groups["witName"].Value,
                                ElementName = missingStateAndTransitionIssueMatch.Groups["elementName"].Value,
                                StateName = missingStateAndTransitionIssueMatch.Groups["stateName"].Value
                            });

                            issuesParsed++;
                        }
                        else
                        {
                            _logger.LogWarning("Issue not parsed: '{entry}'", entry);
                        }
                    }

                    if (Regex.IsMatch(entry, string.Format(EndOfIssuesPattern, projectName)))
                    {
                        var match = Regex.Match(entry, string.Format(EndOfIssuesPattern, projectName));

                        var numberOfIssues = int.Parse(match.Groups["numberOfIssues"].Value);

                        if (numberOfIssues != issuesParsed)
                        {
                            _logger.LogWarning("{issuesNotMatched} issues that were not parsed.", numberOfIssues - issuesParsed);
                        }

                        issuesFound = false;
                        projectName = string.Empty;
                        issuesParsed = 0;
                    }
                }
            }

            return issues;
        }
    }
}