using System.IO.Ports;
using System.Threading;
using OpenQA.Selenium.Appium.Android;
using ZephyrTest.Utils;

namespace ZephyrTest.Core.AppObjects
{
    class Scanner
    {
        //scanner
        private static AndroidDriver<AndroidElement> Driver => AppDriver.GetAndroidDriver();
        private static AndroidElement TextResult => Driver.FindElementById("com.symbol.datawedge:id/output_view");
        private static AndroidElement ButtonScan => Driver.FindElementById("com.symbol.datawedge:id/softscanbutton");

        //barcode
        private static AndroidDriver<AndroidElement> Driver2 => AppDriver.GetAndroidDriver2();
        private static AndroidElement BarcodeInput => Driver2.FindElementById("de.cryptobitch.muelli.barcodegen:id/code");

        public static string ScanBarcode(string barcode)
        {
            ShowBarcode(barcode);
            TextResult.Click();
            PerformScan();
            return GetScanResult();
        }

        private static void ShowBarcode(string barcode)
        {
            BarcodeInput.ReplaceValue(barcode);
        }

        private static void PerformScan()
        {
            var port = new SerialPort("COM4", 9600);
            port.Open();
            port.WriteLine("1");
            port.Close();
            Thread.Sleep(1000);
        }

        private static string GetScanResult()
        {
            return TextResult.Text;
        }
    }
}
