using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;
using System;
using NUnit.Framework.Interfaces;
using WebUITests.NUnit.Utilities;

namespace WebUITests.NUnit
{
    [Parallelizable(ParallelScope.All)]
    public class BaseTest
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        protected ILogger logger;

        [SetUp]
        public void Setup()
        {
            driver = WebDriverSingleton.Driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            var testName = TestContext.CurrentContext.Test.Name;
            logger = LoggerConfig.ConfigureLogger(testName);

            logger.Information("=== Starting test: {TestName} ===", testName);
            logger.Debug("Browser initialized: {BrowserName}", driver.GetType().Name);
        }

        [TearDown]
        public void TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;

            if (testStatus == TestStatus.Failed)
            {
                var errorMessage = TestContext.CurrentContext.Result.Message;
                var stackTrace = TestContext.CurrentContext.Result.StackTrace;

                logger.Error("Test FAILED: {TestName}", testName);
                logger.Error("Error message: {ErrorMessage}", errorMessage);
                logger.Debug("Stack trace: {StackTrace}", stackTrace);

                ScreenshotTaker.TakeScreenshot(driver, logger, testName);
            }
            else
            {
                logger.Information("Test PASSED: {TestName}", testName);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            logger?.Information("Closing browser and cleaning up resources");
            WebDriverSingleton.QuitDriver();
            Log.CloseAndFlush();
        }
    }
}