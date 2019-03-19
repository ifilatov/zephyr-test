using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ZephyrTest.Models.Jira
{
    class TestCycle
    {
        [JsonProperty(PropertyName = "clonedCycleId")]
        public string ClonedCycleId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "build")]
        public string Build { get; set; }
        [JsonProperty(PropertyName = "environment")]
        public string Environment { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "startDate")]
        public string StartDate { get; set; }
        [JsonProperty(PropertyName = "endDate")]
        public string EndDate { get; set; }
        [JsonProperty(PropertyName = "projectId")]
        public string ProjectId { get; set; }
        [JsonProperty(PropertyName = "versionId")]
        public string VersionId { get; set; }

        public static JObject GetCyclePayload(string projectId, string versionId, string name) =>
            JObject.Parse(@"{        
                'clonedCycleId':'',
                'name':'" + name + @"',
                'build':'',
                'environment':'',
                'description':'',
                'startDate':'',
                'endDate':'',
                'projectId': '" + projectId + @"',
                'versionId':'" + versionId + "'}");

        public static JObject GetAddTestsPayload(string projectId, string searchId, string cycleId, string fixVersion) =>
            JObject.Parse(@"{
                'searchId':'" + searchId + @"',
                'fixVersion':'" + fixVersion + @"',
                'cycleId':'" + cycleId +@"',
                'projectId':'" + projectId +@"',
                'method':'2'}");

        public static string GenerateRegressionCycleName(string env) =>
            string.Format("{0}Regression {1}", env, DateTime.Now.ToString());
    }

}
