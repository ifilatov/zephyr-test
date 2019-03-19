using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ZephyrTest.Models.Jira
{
    class JiraIssue
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "self")]
        public string Self { get; set; }
        public static JObject GetIssuePayload(TestExecution execution, TestStep step) =>
            JObject.Parse(@"{
                'fields': {
                'project':
                {
                    'id': '16701'
                },
                'summary': '[BUG] " + execution.IssueKey + " - " + execution.IssueSummary + @"',
                'description': 'Failed on:" + Environment.NewLine + 
                "Test step: " + step.Step + Environment.NewLine + 
                "Test data: " + step.Data + Environment.NewLine + 
                "Expected result: " + step.Result + Environment.NewLine + 
                "Actual result: " + step.ActualResult + @"',
                'customfield_13770': {
                    'id': '12942'
                },
                'issuetype': {
                    'name': 'Bug'
                }
                },   
                'update': {
                    'issuelinks': [
                        {
                            'add': {
                            'type': {
                                'name': 'Bonfire Testing',
                                'inward': 'discovered while testing',
                                'outward': 'testing discovered'
                            },
                            'outwardIssue':{
                                'key': '" + execution.IssueKey + @"'
                            }
                            }
                        }
                    ]
                }    
            }");
    }       

    class LinkedIssue
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }
}