using Newtonsoft.Json;
using System.Collections.Generic;
using ZephyrTest.Enums;

namespace ZephyrTest.Models.Jira
{
    class TestStep
    {
        public string Id { get; set; }
        public string Step { get; set; }
        public string Data { get; set; }
        public string Result { get; set; }
        public string ActualResult {get; set;}
        public Drivers Driver { get; set; }
    }

    class StepResult
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "stepId")]
        public string StepId { get; set; }
        [JsonProperty(PropertyName = "issueId")]
        public string IssueId { get; set; }
        [JsonProperty(PropertyName = "executionId")]
        public string ExecutionId { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }
        [JsonProperty(PropertyName = "defectList")]
        public List<string> DefectList { get; set; }
        [JsonProperty(PropertyName = "updateDefectList")]
        public string UpdateDefectList { get; set; }
    }
}
