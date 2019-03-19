using System;
using System.Drawing.Imaging;
using Allure.Commons;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using ZephyrTest.Enums;
using ZephyrTest.Tools;

namespace ZephyrTest.Utils
{
    class AppDriver
    {
        private static AndroidDriver<AndroidElement> androidDriver;
        private static AndroidDriver<AndroidElement> androidDriver2;
        private static WindowsDriver<WindowsElement> windowsDriver;

        private static IConfigurationRoot Configuration = Configurator.GetConfiguration("AppConfig");

        public static WindowsDriver<WindowsElement> GetWindowsDriver()
        {
            if (windowsDriver == null)
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", Configuration.GetSection("Windows:ApplicationId").Value);
                appCapabilities.SetCapability("deviceName", "WindowsPC");
                windowsDriver = new WindowsDriver<WindowsElement>(new Uri(Configuration.GetSection("Appium:Url").Value + ":4723/wd/hub"), appCapabilities);
                Assert.IsNotNull(windowsDriver);
                windowsDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1.5));
            }
            return windowsDriver;
        }

        public static AndroidDriver<AndroidElement> GetAndroidDriver()
        {
            if (androidDriver == null)
            {
                DesiredCapabilities desiredCaps = new DesiredCapabilities();
                desiredCaps.SetCapability(MobileCapabilityType.DeviceName, Configuration.GetSection("Android:DeviceName").Value);
                desiredCaps.SetCapability(MobileCapabilityType.Udid, Configuration.GetSection("Android:Udid").Value);
                desiredCaps.SetCapability(MobileCapabilityType.PlatformName, Configuration.GetSection("Android:PlatformName").Value);
                desiredCaps.SetCapability(MobileCapabilityType.PlatformVersion, Configuration.GetSection("Android:PlatformVersion").Value);
                desiredCaps.SetCapability(AndroidMobileCapabilityType.AppPackage, Configuration.GetSection("Android:AppPackage").Value);
                desiredCaps.SetCapability(AndroidMobileCapabilityType.AppActivity, Configuration.GetSection("Android:AppActivity").Value);
                androidDriver = new AndroidDriver<AndroidElement>(new Uri(Configuration.GetSection("Appium:Url").Value+":4733/wd/hub"), desiredCaps);
                Assert.IsNotNull(androidDriver);
                androidDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1.5));
            }
            return androidDriver;
        }

        public static AndroidDriver<AndroidElement> GetAndroidDriver2()
        {
            if (androidDriver2 == null)
            {
                DesiredCapabilities desiredCaps = new DesiredCapabilities();
                desiredCaps.SetCapability(MobileCapabilityType.DeviceName, Configuration.GetSection("Android_Barcode:DeviceName").Value);
                desiredCaps.SetCapability(MobileCapabilityType.Udid, Configuration.GetSection("Android_Barcode:Udid").Value);
                desiredCaps.SetCapability(MobileCapabilityType.PlatformName, Configuration.GetSection("Android_Barcode:PlatformName").Value);
                desiredCaps.SetCapability(MobileCapabilityType.PlatformVersion, Configuration.GetSection("Android_Barcode:PlatformVersion").Value);
                desiredCaps.SetCapability(AndroidMobileCapabilityType.AppPackage, Configuration.GetSection("Android_Barcode:AppPackage").Value);
                desiredCaps.SetCapability(AndroidMobileCapabilityType.AppActivity, Configuration.GetSection("Android_Barcode:AppActivity").Value);
                androidDriver2 = new AndroidDriver<AndroidElement>(new Uri(Configuration.GetSection("Appium:Url").Value + ":4743/wd/hub"), desiredCaps);
                Assert.IsNotNull(androidDriver2);
                androidDriver2.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1.5));
            }
            return androidDriver2;
        }

        public static void OneTimeTearDown()
        {
            if (androidDriver != null)
            {
                androidDriver.Quit();
                androidDriver = null;
            }
            if (windowsDriver != null)
            {
                windowsDriver.Quit();
                windowsDriver = null;
            }
            if (androidDriver2 != null)
            {
                androidDriver2.Quit();
                androidDriver2 = null;
            }
        }

        public static void AllureAttachScreenshot(string path)
        {
            AllureLifecycle.Instance.AddAttachment(path);
        }

        public static string SaveScreenshot(string name, Drivers driver){
            name = string.Format("allure-results/{0}.png", name);
            switch (driver)
            {
                case Drivers.Windows: GetWindowsDriver().GetScreenshot().SaveAsFile(name, ImageFormat.Png); break;
                case Drivers.Android: GetAndroidDriver().GetScreenshot().SaveAsFile(name, ImageFormat.Png); break;
            }
            return name;
        }
    }
}