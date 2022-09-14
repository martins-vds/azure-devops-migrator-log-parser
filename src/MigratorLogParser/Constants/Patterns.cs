using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigratorLogParser.Constants
{
    public static class Patterns
    {
        public const string MissingPermission = @"(?<description>(?<issueRef>ISVError:\d+) An expected permission is missing. Missing Permission:(?<permission>\S*)\s*for Group:(?<group>S(-\d+){7}-0-0-0-0-\d) and Scope:(?<scope>[{]?[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}[}]?))";
        public const string InvalidIdentity = @"(?<description>User(?:.*?)Identifier: (?<sid>S(-\d+){7}); DisplayName: (?<displayName>[^)]+)(?:.*?)property \[(?<property>[^]]+)\] contained invalid characters)";
        public const string GlobalList = @"(?<description>Global List : (?<globalList>.*?(?=\s*will)) will be excluded from import.)";
        public const string ProcessValidation = @"\[Info[^\]]*?\] Step : ProcessValidation INFO";
        public const string ProcessValidationIssue = @"\[Error[^\]]*?\] Step : ProcessValidation - Failure Type - Validation failed : Invalid process template: (?<file>[^:]*):(?<lineNumber>\d*):";
    }
}
