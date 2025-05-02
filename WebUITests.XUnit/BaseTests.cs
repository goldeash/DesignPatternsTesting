using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using WebUITests.XUnit.Utilities;

namespace WebUITests.XUnit
{
    public class BaseTest : IDisposable
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        protected ILogger logger;
        protected readonly ITestOutputHelper output;

        public BaseTest(ITestOutputHelper output)
        {
            this.output = output;
            driver = WebDriverSingleton.Driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            var testName = output.GetType().FullName;
            logger = LoggerConfig.ConfigureLogger(testName);

            logger.Information("=== Starting test: {TestName} ===", testName);
            logger.Debug("Browser initialized: {BrowserName}", driver.GetType().Name);
        }

        public void Dispose()
        {
            var testName = output.GetType().FullName;
            logger.Information("Test completed: {TestName}", testName);
        }

        protected void LogTestFailure(string errorMessage, string stackTrace)
        {
            var testName = output.GetType().FullName;
            logger.Error("Test FAILED: {TestName}", testName);
            logger.Error("Error message: {ErrorMessage}", errorMessage);
            logger.Debug("Stack trace: {StackTrace}", stackTrace);

            ScreenshotTaker.TakeScreenshot(driver, logger, testName);
        }
    }

    public class TestCollectionFixture : IDisposable
    {
        public TestCollectionFixture()
        {
        }

        public void Dispose()
        {
            Log.Information("Closing browser and cleaning up resources");
            WebDriverSingleton.QuitDriver();
            Log.CloseAndFlush();
        }
    }
}