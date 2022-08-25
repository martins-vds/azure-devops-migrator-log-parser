using FluentAssertions;
using Microsoft.Extensions.Logging;
using MigratorLogParser.Models;
using MigratorLogParser.Models.ProcessValidationIssues;
using System.IO.Abstractions.TestingHelpers;
using System.Text;

namespace MigratorLogParser.Tests
{
    public class LogParserTests
    {
        [Fact]
        public void It_Parses_TF402583()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\file.log", new MockFileData(BuildLogsTF402583Issues()) }
            });

            var parser = new LogParser(fileSystem, new LoggerFactory().CreateLogger<LogParser>());

            var migrationLog = parser.Parse(@"c:\file.log");

            migrationLog.ProcessValidationIssues.Should().BeEquivalentTo(new List<ProcessValidation>()
            {
                new ProcessValidation()
                {
                    ProjectName = "ADMS-P-SGPA-P",
                    Issues = new List<ProcessValidationIssue>()
                    {
                        new TF402583(){ File = "WorkItem Tracking\\LinkTypes\\Scrum.ImpededBy.xml", LineNumber = 2, Description = "TF402583: Custom link type Scrum.ImpededBy is invalid because custom link types aren't supported.", CustomLink = "Scrum.ImpededBy", IssueRef = "TF402583" },
                        new TF402583(){ File = "WorkItem Tracking\\LinkTypes\\Scrum.ImplementedBy.xml", LineNumber = 2, Description = "TF402583: Custom link type Scrum.ImplementedBy is invalid because custom link types aren't supported.", CustomLink = "Scrum.ImplementedBy", IssueRef = "TF402583" },
                        new TF402583(){ File = "WorkItem Tracking\\LinkTypes\\Scrum.FailedBy.xml", LineNumber = 2, Description = "TF402583: Custom link type Scrum.FailedBy is invalid because custom link types aren't supported.", CustomLink = "Scrum.FailedBy", IssueRef = "TF402583" }
                    }
                }
            });
        }

        [Fact]
        public void It_Parses_TF402544()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\file.log", new MockFileData(BuildLogsWithTF402544Issues()) }
            });

            var parser = new LogParser(fileSystem, new LoggerFactory().CreateLogger<LogParser>());

            var migrationLog = parser.Parse(@"c:\file.log");

            migrationLog.ProcessValidationIssues.Should().BeEquivalentTo(new List<ProcessValidation>()
            {
                new ProcessValidation()
                {
                    ProjectName = "ADMS-P-SGPA-P",
                    Issues = new List<ProcessValidationIssue>()
                    {
                        new TF402544(){ File = "WorkItem Tracking\\TypeDefinitions\\FeedbackRequestDemandederetour.xml", LineNumber = 75, RefName = "Microsoft.VSTS.Feedback.ApplicationType", WitName = "Feedback Request-Demande de retour", ElementName = "FeedbackRequestWorkItems", Description = "TF402544: Field Microsoft.VSTS.Feedback.ApplicationType, defined in work item type Feedback Request-Demande de retour, requires an ALLOWEDVALUES rule that contains values to support element FeedbackRequestWorkItems specified in ProcessConfiguration.", IssueRef = "TF402544" },
                    }
                }
            });
        }

        [Fact]
        public void It_Parses_TF402551()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\file.log", new MockFileData(BuildLogsWithTF402551Issues()) }
            });

            var parser = new LogParser(fileSystem, new LoggerFactory().CreateLogger<LogParser>());

            var migrationLog = parser.Parse(@"c:\file.log");

            migrationLog.ProcessValidationIssues.Should().BeEquivalentTo(new List<ProcessValidation>()
            {
                new ProcessValidation()
                {
                    ProjectName = "ADMS-P-SGPA-P",
                    Issues = new List<ProcessValidationIssue>()
                    {
                        new TF402551(){ File = "WorkItem Tracking\\TypeDefinitions\\CodeReviewRequestDemandedervisiondecode.xml", LineNumber = -1, WitName = "Code Review Request-Demande de révision de code", ElementName = "Microsoft.CodeReviewRequestCategory", Description = "TF402551: Work item type Code Review Request-Demande de révision de code doesn't define workflow state Requested, which is required because ProcessConfiguration maps it to a metastate for element Microsoft.CodeReviewRequestCategory.", IssueRef = "TF402551" },
                        new TF402551(){ File = "WorkItem Tracking\\TypeDefinitions\\CodeReviewResponseRponservisiondecode.xml", LineNumber = -1, WitName = "Code Review Response-Réponse à révision de code", ElementName = "Microsoft.CodeReviewResponseCategory", Description = "TF402551: Work item type Code Review Response-Réponse à révision de code doesn't define workflow state Requested, which is required because ProcessConfiguration maps it to a metastate for element Microsoft.CodeReviewResponseCategory.", IssueRef = "TF402551" },
                    }
                }

            });
        }

        private string BuildLogsTF402583Issues()
        {
            var builder = new StringBuilder();

            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - Starting validation of project 8=ADMS-P-SGPA-P, process=c:\temp\pkrr5sy0.ylm\ADMSPSGPAP.zip");
            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - AllowCustomTeamField: False.");
            builder.AppendLine(@"[Error] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: WorkItem Tracking\LinkTypes\Scrum.ImpededBy.xml:2: TF402583: Custom link type Scrum.ImpededBy is invalid because custom link types aren't supported.");
            builder.AppendLine(@"[Error] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: WorkItem Tracking\LinkTypes\Scrum.ImplementedBy.xml:2: TF402583: Custom link type Scrum.ImplementedBy is invalid because custom link types aren't supported.");
            builder.AppendLine(@"[Error] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: WorkItem Tracking\LinkTypes\Scrum.FailedBy.xml:2: TF402583: Custom link type Scrum.FailedBy is invalid because custom link types aren't supported.");
            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - Validation failed for project ADMS-P-SGPA-P with 3 errors");

            return builder.ToString();
        }

        private string BuildLogsWithTF402544Issues()
        {
            var builder = new StringBuilder();

            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - Starting validation of project 8=ADMS-P-SGPA-P, process=c:\temp\pkrr5sy0.ylm\ADMSPSGPAP.zip");
            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - AllowCustomTeamField: False.");
            builder.AppendLine(@"[Error] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: WorkItem Tracking\TypeDefinitions\FeedbackRequestDemandederetour.xml:75: TF402544: Field Microsoft.VSTS.Feedback.ApplicationType, defined in work item type Feedback Request-Demande de retour, requires an ALLOWEDVALUES rule that contains values to support element FeedbackRequestWorkItems specified in ProcessConfiguration.");
            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - Validation failed for project ADMS-P-SGPA-P with 1 errors");

            return builder.ToString();
        }

        private string BuildLogsWithTF402551Issues()
        {
            var builder = new StringBuilder();

            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - Starting validation of project 8=ADMS-P-SGPA-P, process=c:\temp\pkrr5sy0.ylm\ADMSPSGPAP.zip");
            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - AllowCustomTeamField: False.");
            builder.AppendLine(@"[Error] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: WorkItem Tracking\TypeDefinitions\CodeReviewRequestDemandedervisiondecode.xml:: TF402551: Work item type Code Review Request-Demande de révision de code doesn't define workflow state Requested, which is required because ProcessConfiguration maps it to a metastate for element Microsoft.CodeReviewRequestCategory.");
            builder.AppendLine(@"[Error] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: WorkItem Tracking\TypeDefinitions\CodeReviewResponseRponservisiondecode.xml:: TF402551: Work item type Code Review Response-Réponse à révision de code doesn't define workflow state Requested, which is required because ProcessConfiguration maps it to a metastate for element Microsoft.CodeReviewResponseCategory.");
            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - Validation failed for project ADMS-P-SGPA-P with 2 errors");

            return builder.ToString();
        }
    }
}