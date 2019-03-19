using System.Collections.Generic;
using RestSharp;
using ZephyrTest.Models.Jira;
using Newtonsoft.Json;
using NUnit.Framework;
using ZephyrTest.Utils;
using log4net;
using Newtonsoft.Json.Linq;

namespace ZephyrTest.Core
{
    class TestManager
    {
        public static string CycleName;
        public static string Env;
        private static readonly ILog Log = LogManager.GetLogger(typeof(TestManager));

        public static TestStep GetStepById(string issueId, string stepId)
        {
            IRestRequest request = Zephyr.GetAuthorizedRequest(string.Format("{0}/{1}/{2}", Zephyr.GetUrlValue("StepUrl"), issueId, stepId), Method.GET);
            IRestResponse<TestStep> response = Zephyr.Execute<TestStep>(request);
            //Log.Debug(JsonConvert.SerializeObject(response));
            return JsonConvert.DeserializeObject<TestStep>(response.Content);
        }

        public static List<StepResult> GetStepResults(TestExecution execution)
        {
            InitializeStepResults(execution);
            IRestRequest request = Zephyr.GetAuthorizedRequest(string.Format("{0}?executionId={1}", Zephyr.GetUrlValue("StepResultUrl"), execution.Id), Method.GET);
            IRestResponse<List<StepResult>> response = Zephyr.Execute<List<StepResult>>(request);
            //Log.Debug(JsonConvert.SerializeObject(response));
            return JsonConvert.DeserializeObject<List<StepResult>>(response.Content);
        }

        public static List<TestCaseData> TestCases
        {
            get
            {
                if (string.IsNullOrEmpty(TestContext.Parameters["env"]))
                {
                    Env = "";
                }
                else
                {
                    Env = TestContext.Parameters["env"] + ": ";
                }
                if (string.IsNullOrEmpty(TestContext.Parameters["cycleName"]))
                {
                    CycleName = TestCycle.GenerateRegressionCycleName(Env);
                    AddTestsToRegressionCycle();
                }
                else
                {
                    CycleName = TestContext.Parameters["cycleName"];
                }
                List<TestCaseData> testCases = new List<TestCaseData>();
                IRestRequest request = Zephyr.GetAuthorizedRequest(string.Format("{0}?zqlQuery=project=CRTM+AND+fixVersion=\"{1}\"+AND+cycleName=\"{2}\"", Zephyr.GetUrlValue("ExecutionsUrl"), "Unscheduled", CycleName), Method.GET);
                IRestResponse<ExecutionsList> response = Zephyr.Execute<ExecutionsList>(request);
                response.Data.Executions.ForEach(execution =>
                {
                    TestCaseData testCase = new TestCaseData(execution.Id, execution).SetName(execution.IssueKey + " - " + execution.IssueSummary);
                    testCases.Add(testCase);
                });
                return testCases;
            }
        }

        private static void InitializeStepResults(TestExecution execution)
        {
            IRestRequest request = Zephyr.GetAuthorizedRequest(string.Format("{0}/{1}?expand=checksteps", Zephyr.GetUrlValue("ExecutionUrl"), execution.Id), Method.GET);
            IRestResponse response = Zephyr.Execute(request);
            //Log.Debug(JsonConvert.SerializeObject(response));
        }

        private static string GetVersionByName(string versionName)
        {
            IRestRequest request = Zephyr.GetAuthorizedRequest(string.Format("{0}?projectId={1}", Zephyr.GetUrlValue("VersionsUrl"), Zephyr.GetIdValue("ProjectId")), Method.GET);
            IRestResponse<VersionBoard> response = Zephyr.Execute<VersionBoard>(request);
            Log.Debug("GetVersionByName");
            Log.Debug(JsonConvert.SerializeObject(response.Data));
            return response.Data.UnreleasedVersions.Find(x => x.Label == versionName).Value;
        }
        private static string InitializeRegressionCycle()
        {
            IRestRequest request = Zephyr.GetAuthorizedRequest(Zephyr.GetUrlValue("TestCycleUrl"), Method.POST);
            JObject payload = TestCycle.GetCyclePayload(Zephyr.GetIdValue("ProjectId"), GetVersionByName(TestContext.Parameters["fixVersion"]), CycleName);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(payload), ParameterType.RequestBody);
            IRestResponse<JObject> response = Zephyr.Execute<JObject>(request);
            JObject data = JObject.Parse(response.Content);
            Log.Debug("InitializeRegressionCycle");
            Log.Debug(JsonConvert.SerializeObject(data));
            return (string)data["id"];
        }

        private static void AddTestsToRegressionCycle()
        {
            var cycleId = InitializeRegressionCycle();
            IRestRequest request = Zephyr.GetAuthorizedRequest(string.Format("{0}/addTestsToCycle", Zephyr.GetUrlValue("ExecutionUrl")), Method.POST);
            JObject payload = TestCycle.GetAddTestsPayload(Zephyr.GetIdValue("ProjectId"), Zephyr.GetIdValue("TestFilterId"), cycleId, TestContext.Parameters["fixVersion"]);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(payload), ParameterType.RequestBody);
            IRestResponse response = Zephyr.Execute(request);
            Log.Debug("AddTestsToRegressionCycle");
            Log.Debug(JsonConvert.SerializeObject(response));
        }
    }
}
