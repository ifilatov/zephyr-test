using NUnit.Framework;
using ZephyrTest.Models.Jira;
using System.Collections.Generic;
using ZephyrTest.Core.Steps;
using NUnit.Allure.Core;
using ZephyrTest.Utils;
using log4net;
using ZephyrTest.Enums;
using ZephyrTest.Models;
using System;

namespace ZephyrTest.Core
{
    [AllureNUnit]
    [TestFixture]
    class TestRunner
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TestReporter));

        //[Test]
        //public void Test()
        //{
        //    var result = SAPSteps.ValidateFileImport("[{\"filename\": \"c:/import/sites_SR0.csv\"},{\"filename\": \"c:/import/sites_SR0.csv\"}]");
        //    Console.WriteLine(result);
        //}

        [Test]
        [TestCaseSource(typeof(TestManager), "TestCases")]
        public void RunTest(string testId, TestExecution execution)
        {
            NunitTest test = new NunitTest();
            List<StepResult> stepResults = TestManager.GetStepResults(execution);
            stepResults.ForEach(stepResult =>
            {
                TestStep step = TestManager.GetStepById(execution.IssueId, stepResult.StepId);
                ExecuteStep(step);
                if (step.ActualResult.Contains(step.Result))
                {
                    TestReporter.OnStepPass(execution, step, stepResult);
                }
                else
                {
                    if (!test.Failed)
                    {
                        test.Failed = true;
                        test.ActualResult = step.ActualResult;
                        test.ExpectedResult = step.Result;
                    }
                    stepResult.Comment = "AR: " + step.ActualResult;
                    TestReporter.OnStepFail(execution, step, stepResult);
                }
            });
            Assert.AreEqual(test.ExpectedResult, test.ActualResult);
            TestReporter.SetTestExecutionStatus(execution, Results.PASS);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            AppDriver.OneTimeTearDown();
        }

        private static readonly Dictionary<string, StepBinding> StepDefinitions = new Dictionary<string, StepBinding>
        {
            { "Track Package", new StepBinding{Driver = Drivers.Windows, StepReference = TrackSteps.TrackPackage } },
            { "Scan Barcode", new StepBinding{Driver = Drivers.Android, StepReference = ScanSteps.ScanBarcode } },
            { "Create Request", new StepBinding{Driver = Drivers.None, StepReference = WebServiceSteps.CreateRequest } },
            { "Read Request", new StepBinding{Driver = Drivers.None, StepReference = WebServiceSteps.ReadRequest } },
            { "Update Request", new StepBinding{Driver = Drivers.None, StepReference = WebServiceSteps.UpdateRequest } },
            { "Delete Request", new StepBinding{Driver = Drivers.None, StepReference = WebServiceSteps.DeleteRequest } },
            { "Read All Request", new StepBinding{Driver = Drivers.None, StepReference = WebServiceSteps.ReadAll} },
            { "Check Availability", new StepBinding{Driver = Drivers.None, StepReference = WebServiceSteps.HealthCheck } },

            //real life
            { "Validate CSV import", new StepBinding{ Driver = Drivers.None, StepReference = SAPSteps.ValidateFileImport} }
        };

        private void ExecuteStep(TestStep x)
        {
            x.Driver = StepDefinitions[x.Step].Driver;
            x.ActualResult = StepDefinitions[x.Step].StepReference(x.Data);
        }
    }
}