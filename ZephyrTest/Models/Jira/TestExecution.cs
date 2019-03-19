using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZephyrTest.Models.Jira
{
    class ExecutionsList
    {
        public List<TestExecution> Executions { get; set; }
    }

    class ExecutionResult
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }

    class TestExecution
    {
        public string Id { get; set; }
        public string ExecutionStatus { get; set; }
        public string Comment { get; set; }
        public string CycleId { get; set; }
        public string CycleName { get; set; }
        public string VersionId { get; set; }
        public string VersionName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectKey { get; set; }
        public string IssueId { get; set; }
        public string IssueKey { get; set; }
        public string IssueSummary { get; set; }
        public string Label { get; set; }
        public string Component { get; set; }
    }
}
