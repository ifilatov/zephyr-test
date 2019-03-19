using OpenQA.Selenium.Appium.Windows;
using ZephyrTest.Utils;

namespace ZephyrTest.Core.AppObjects
{
    class PackageTracker
    {
        private static WindowsDriver<WindowsElement> Driver => AppDriver.GetWindowsDriver();
        private static WindowsElement InputTrackPackage => Driver.FindElementByAccessibilityId("TrackPackageTextBox");
        private static WindowsElement ButtonTrack => Driver.FindElementByAccessibilityId("ButtonTrack");
        private static WindowsElement TextResult => Driver.FindElementByAccessibilityId("PackageInfoTextBlock");

        public static string TrackPackage(string id)
        {
            Clear();
            InputTrackPackage.SendKeys(id);
            ButtonTrack.Click();
            return GetCalculatorResultText();
        }

        private static void Clear()
        {
            InputTrackPackage.Clear();
        }

        private static string GetCalculatorResultText()
        {
            return TextResult.Text.Trim();
        }
    }
}
