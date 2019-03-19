using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using ZephyrTest.Utils;

namespace ZephyrTest.Core.AppObjects
{
    class Calculator
    {
        private static WindowsDriver<WindowsElement> Driver => AppDriver.GetWindowsDriver();
        private static WindowsElement CalculatorResult => Driver.FindElementByAccessibilityId("CalculatorResults");
        private static WindowsElement ButtonPlus => Driver.FindElementByName("Plus");
        private static WindowsElement ButtonMinus => Driver.FindElementByName("Minus");
        private static WindowsElement ButtonDivide => Driver.FindElementByName("Divide by");
        private static WindowsElement ButtonMultiply => Driver.FindElementByName("Multiply by");
        private static WindowsElement ButtonEquals => Driver.FindElementByName("Equals");

        private static readonly Dictionary<string, string> numbers = new Dictionary<string, string>
        {
            { "1", "One" },
            { "2", "Two" },
            { "3", "Three" },
            { "4", "Four" },
            { "5", "Five" },
            { "6", "Six" },
            { "7", "Seven" },
            { "8", "Eight" },
            { "9", "Nine" },
            { "0", "Zero" }
        };

        public static string Add(string n1, string n2)
        {
            Clear();
            Driver.FindElementByName(numbers[n1]).Click();
            ButtonPlus.Click();
            Driver.FindElementByName(numbers[n2]).Click();
            ButtonEquals.Click();
            return GetCalculatorResultText();
        }

        public static string Subtract(string n1, string n2)
        {
            Clear();
            Driver.FindElementByName(numbers[n1]).Click();
            ButtonMinus.Click();
            Driver.FindElementByName(numbers[n2]).Click();
            ButtonEquals.Click();
            return GetCalculatorResultText();
        }

        public static string Divide(string n1, string n2)
        {
            Clear();
            Driver.FindElementByName(numbers[n1]).Click();
            ButtonDivide.Click();
            Driver.FindElementByName(numbers[n2]).Click();
            ButtonEquals.Click();
            return GetCalculatorResultText();
        }

        public static string Multiply(string n1, string n2)
        {
            Clear();
            Driver.FindElementByName(numbers[n1]).Click();
            ButtonMultiply.Click();
            Driver.FindElementByName(numbers[n2]).Click();
            ButtonEquals.Click();
            return GetCalculatorResultText();
        }

        private static void Clear()
        {
            Driver.FindElementByName("Clear").Click();
            Assert.AreEqual("0", GetCalculatorResultText());
        }

        private static string GetCalculatorResultText()
        {
            return CalculatorResult.Text.Replace("Display is", string.Empty).Trim();
        }
    }
}
