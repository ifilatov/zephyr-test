# Zephyr scriptless automation
Keyword-driven test framework for Desktop scriptless functional testing (can be extended to Mobile). 
Uses Jira-Zephyr as test storage and to aggregate results.
Uses Appium to handle desktop application.

# Installation

## Requirement

* JRE
* .Net
* git
* nuget
* allure
* Node.js
* npm install -g appium

## Build and run tests

* checkout git repo
* nuget.exe restore .\ZephyrTest.sln
* dotnet build
* .\packages\NUnit.ConsoleRunner.3.9.0\tools\nunit3-console .\ZephyrTest.sln -p:fixVersion="Unscheduled" -p:cycleName="Ad Hoc"
* allure generate -o .\allure-results\report .\allure-results\

Note that framework expects that Appium server specified in Configs\WinApp.json is up and running. Application will be started automatically.

## Test parameters (will be also required in CI job via "parameterized build")
fixVersion - name of fix version, e.g. "Unscheduled", "May 2019"
cycleName - name of test cycle within fix version, e.g. "Ad hoc"

# Continuous Integration

## TBD

# Test Results

## For functional testers
* Test Cases and steps are marked as PASSED/FAILED in Jira. Actual reporting will be configured according to test strategy.

## For automation engineers
* NUnit test results saved to .\TestResult.xml
* Allure report saved to .\allure-results\report (open index.html). Please note that Chrome does not open local report correctly due to cross-scripting (use FF, Edge or open remote one)