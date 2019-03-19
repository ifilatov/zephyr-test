using System.Collections.Generic;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using ZephyrTest.Models.Jira;
using ZephyrTest.Utils;
using NUnit.Framework;
using System.IO;
using System.Linq;
using ZephyrTest.Enums;

namespace ZephyrTest.Core
{
    class TestReporter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TestReporter));

        public static void OnStepFail(TestExecution execution, TestStep step, StepResult stepResult){          
            string screenshotPath = "";
            //attach screenshot
            if(step.Driver != Drivers.None)
            {
                screenshotPath = AppDriver.SaveScreenshot(TestContext.CurrentContext.Test.Name, step.Driver);
                AppDriver.AllureAttachScreenshot(screenshotPath);
                AttachScreenshotToStep(stepResult, screenshotPath);
            }
            //create jira issue if it does not exist
            if (CheckBugExistance(execution) == false)
            {
                string issueKey = CreateJiraIssue(execution, step);
                if (!string.IsNullOrEmpty(screenshotPath)) { AttachScreenshotToIssue(issueKey, screenshotPath); };
                stepResult.DefectList = new List<string> { issueKey };
                stepResult.UpdateDefectList = "true";
            }
            //set test execution result
            SetStepStatus(step, stepResult, Results.FAIL);
            SetTestExecutionStatus(execution, Results.FAIL);
        }

        public static void OnStepPass(TestExecution execution, TestStep step, StepResult stepResult){
            SetStepStatus(step, stepResult, Results.PASS);
        }
        
        internal static void SetStepStatus(TestStep step, StepResult stepResult, Results status)
        {
            stepResult.Status = status.ToString("D");
            IRestRequest request = Zephyr.GetAuthorizedRequest(string.Format("{0}/{1}", Zephyr.GetUrlValue("StepResultUrl"), stepResult.Id), Method.PUT);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(stepResult), ParameterType.RequestBody);
            IRestResponse response = Zephyr.Execute(request);
            //Log.Debug(JsonConvert.SerializeObject(response));
            Log.Info(string.Format("Step result \"{0} - {1}\" set to {2}", stepResult.Id, step.Step, status));
        }

        internal static void SetTestExecutionStatus(TestExecution execution, Results status)
        {
            ExecutionResult executionResult = new ExecutionResult
            {
                Status = status.ToString("D")
            };
            IRestRequest request = Zephyr.GetAuthorizedRequest(string.Format("{0}/{1}/execute", Zephyr.GetUrlValue("ExecutionUrl"), execution.Id), Method.PUT);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(executionResult), ParameterType.RequestBody);
            IRestResponse response = Zephyr.Execute(request);
            //Log.Debug(JsonConvert.SerializeObject(response));
            Log.Info(string.Format("Test Execution result \"{0} - {1}\" set to {2}", execution.Id, execution.IssueSummary, status));
        }

        internal static string CreateJiraIssue(TestExecution execution, TestStep step)
        {
            IRestRequest request = Zephyr.GetAuthorizedRequest(Zephyr.GetUrlValue("CreateJiraIssueUrl"), Method.POST);
            JObject payload = JiraIssue.GetIssuePayload(execution, step);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(payload), ParameterType.RequestBody);
            IRestResponse<JiraIssue> response = Zephyr.Execute<JiraIssue>(request);
            //Log.Debug(JsonConvert.SerializeObject(response));
            Log.Info(string.Format("Created Jira Issue \"{0} - {1}\"", response.Data.Key, "[BUG]" + execution.IssueKey + " - " + execution.IssueSummary));
            return response.Data.Key;
        }

        internal static void AttachScreenshotToIssue(string issueKey, string path){
            //Log.Debug(issueKey);
            IRestRequest request = Zephyr.GetAuthorizedRequest(string.Format("{0}/{1}/attachments", Zephyr.GetUrlValue("CreateJiraIssueUrl"), issueKey), Method.POST);
            request.AddHeader("X-Atlassian-Token", "nocheck");
            byte[] file = File.ReadAllBytes(path);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFileBytes("file", file, "Screenshot.png", "application/octet-stream");
            IRestResponse response = Zephyr.Execute(request);
            //Log.Debug(JsonConvert.SerializeObject(response));
        }

        internal static void AttachScreenshotToStep(StepResult stepResult, string path)
        {
            IRestRequest request = Zephyr.GetAuthorizedRequest(string.Format("{0}?entityType=TESTSTEPRESULT&entityId={1}", Zephyr.GetUrlValue("AttachFileToStepUrl"), stepResult.Id), Method.POST);
            request.AddHeader("X-Atlassian-Token", "nocheck");
            byte[] file = File.ReadAllBytes(path);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFileBytes("file", file, "Screenshot.png", "application/octet-stream");
            IRestResponse response = Zephyr.Execute(request);
            //Log.Debug(JsonConvert.SerializeObject(response));
        }

        internal static bool CheckBugExistance(TestExecution execution)
        {
            IRestRequest request = Zephyr.GetAuthorizedRequest(string.Format("{0}/{1}?fields=summary,issuelinks",Zephyr.GetUrlValue("CreateJiraIssueUrl"), execution.IssueId), Method.GET);
            IRestResponse<JObject> response = Zephyr.Execute<JObject>(request);
            JObject data = JObject.Parse(response.Content);
            return ((JArray)data["fields"]["issuelinks"]).Select(x => new LinkedIssue
            {
                Status = (string)x["inwardIssue"]["fields"]["status"]["name"],
                Type = (string)x["inwardIssue"]["fields"]["issuetype"]["name"]
            }).ToList().Exists(x => x.Status != "Done" && x.Type == "Bug");
        }

    }
}
