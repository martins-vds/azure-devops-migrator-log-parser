using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Exceptions;
using System.IO.Abstractions.TestingHelpers;
using System.Text;

namespace MigratorLogParser.Tests
{
    public class LogParserTests
    {
        [Fact]
        public void It_Parses_Custom_Link_Issues()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\file.log", new MockFileData(BuildLogsWithCustomLinkIssues()) }
            });

            var parser = new LogParser(fileSystem, new LoggerFactory().CreateLogger<LogParser>());

            var issues = parser.ParseProcessValidationIssues(@"c:\file.log");

            issues.Should().BeEquivalentTo(new List<ProcessValidationIssue>()
            {
                new CustomLinkIssue(){ ProjectName = "ADMS-P-SGPA-P", File = "WorkItem Tracking\\LinkTypes\\Scrum.ImpededBy.xml", LineNumber = 2, Description = "TF402583: Custom link type Scrum.ImpededBy is invalid because custom link types aren't supported.", CustomLink = "Scrum.ImpededBy", IssueRef = "TF402583", Remediation = string.Empty },
                new CustomLinkIssue(){ ProjectName = "ADMS-P-SGPA-P", File = "WorkItem Tracking\\LinkTypes\\Scrum.ImplementedBy.xml", LineNumber = 2, Description = "TF402583: Custom link type Scrum.ImplementedBy is invalid because custom link types aren't supported.", CustomLink = "Scrum.ImplementedBy", IssueRef = "TF402583", Remediation = string.Empty },
                new CustomLinkIssue(){ ProjectName = "ADMS-P-SGPA-P", File = "WorkItem Tracking\\LinkTypes\\Scrum.FailedBy.xml", LineNumber = 2, Description = "TF402583: Custom link type Scrum.FailedBy is invalid because custom link types aren't supported.", CustomLink = "Scrum.FailedBy", IssueRef = "TF402583", Remediation = string.Empty }
            });
        }

        [Fact]
        public void It_Parses_Missing_Allowed_Values_Issues()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\file.log", new MockFileData(BuildLogsWithMissingAllowedValuesIssues()) }
            });

            var parser = new LogParser(fileSystem, new LoggerFactory().CreateLogger<LogParser>());

            var issues = parser.ParseProcessValidationIssues(@"c:\file.log");

            issues.Should().BeEquivalentTo(new List<ProcessValidationIssue>()
            {
                new MissingAllowedValuesIssue(){ ProjectName = "ADMS-P-SGPA-P", File = "WorkItem Tracking\\TypeDefinitions\\FeedbackRequestDemandederetour.xml", LineNumber = 75, Remediation = string.Empty, RefName = "Microsoft.VSTS.Feedback.ApplicationType", WitName = "Feedback Request-Demande de retour", ElementName = "FeedbackRequestWorkItems", Description = "TF402544: Field Microsoft.VSTS.Feedback.ApplicationType, defined in work item type Feedback Request-Demande de retour, requires an ALLOWEDVALUES rule that contains values to support element FeedbackRequestWorkItems specified in ProcessConfiguration.", IssueRef = "TF402544" },
            });
        }

        [Fact]
        public void It_Parses_Missing_State_Transition_Issues()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\file.log", new MockFileData(BuildLogsWithMissingStateAndTransitionIssues()) }
            });

            var parser = new LogParser(fileSystem, new LoggerFactory().CreateLogger<LogParser>());

            var issues = parser.ParseProcessValidationIssues(@"c:\file.log");

            issues.Should().BeEquivalentTo(new List<ProcessValidationIssue>()
            {
                new MissingStateAndTransitionIssue(){ ProjectName = "ADMS-P-SGPA-P", File = "WorkItem Tracking\\TypeDefinitions\\CodeReviewRequestDemandedervisiondecode.xml", LineNumber = -1, Remediation = string.Empty, WitName = "Code Review Request-Demande de r�vision de code", ElementName = "Microsoft.CodeReviewRequestCategory", Description = "TF402551: Work item type Code Review Request-Demande de r�vision de code doesn't define workflow state Requested, which is required because ProcessConfiguration maps it to a metastate for element Microsoft.CodeReviewRequestCategory.", IssueRef = "TF402551" },
                new MissingStateAndTransitionIssue(){ ProjectName = "ADMS-P-SGPA-P", File = "WorkItem Tracking\\TypeDefinitions\\CodeReviewResponseRponservisiondecode.xml", LineNumber = -1, Remediation = string.Empty, WitName = "Code Review Response-R�ponse � r�vision de code", ElementName = "Microsoft.CodeReviewResponseCategory", Description = "TF402551: Work item type Code Review Response-R�ponse � r�vision de code doesn't define workflow state Requested, which is required because ProcessConfiguration maps it to a metastate for element Microsoft.CodeReviewResponseCategory.", IssueRef = "TF402551" },
            });
        }

        private string BuildLogsWithCustomLinkIssues()
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

        private string BuildLogsWithMissingAllowedValuesIssues()
        {
            var builder = new StringBuilder();

            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - Starting validation of project 8=ADMS-P-SGPA-P, process=c:\temp\pkrr5sy0.ylm\ADMSPSGPAP.zip");
            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - AllowCustomTeamField: False.");
            builder.AppendLine(@"[Error] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: WorkItem Tracking\TypeDefinitions\FeedbackRequestDemandederetour.xml:75: TF402544: Field Microsoft.VSTS.Feedback.ApplicationType, defined in work item type Feedback Request-Demande de retour, requires an ALLOWEDVALUES rule that contains values to support element FeedbackRequestWorkItems specified in ProcessConfiguration.");
            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - Validation failed for project ADMS-P-SGPA-P with 1 errors");

            return builder.ToString();
        }

        private string BuildLogsWithMissingStateAndTransitionIssues()
        {
            var builder = new StringBuilder();

            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - Starting validation of project 8=ADMS-P-SGPA-P, process=c:\temp\pkrr5sy0.ylm\ADMSPSGPAP.zip");
            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - AllowCustomTeamField: False.");
            builder.AppendLine(@"[Error] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: WorkItem Tracking\TypeDefinitions\CodeReviewRequestDemandedervisiondecode.xml:: TF402551: Work item type Code Review Request-Demande de r�vision de code doesn't define workflow state Requested, which is required because ProcessConfiguration maps it to a metastate for element Microsoft.CodeReviewRequestCategory.");
            builder.AppendLine(@"[Error] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: WorkItem Tracking\TypeDefinitions\CodeReviewResponseRponservisiondecode.xml:: TF402551: Work item type Code Review Response-R�ponse � r�vision de code doesn't define workflow state Requested, which is required because ProcessConfiguration maps it to a metastate for element Microsoft.CodeReviewResponseCategory.");
            builder.AppendLine(@"[Info] Step : ProcessValidation INFO - Validation failed for project ADMS-P-SGPA-P with 2 errors");

            return builder.ToString();
        }
    }
}