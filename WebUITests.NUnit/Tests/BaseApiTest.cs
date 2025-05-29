using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Serilog;
using WebUITests.NUnit.Utilities;

namespace WebUITests.NUnit.Tests
{
    [AllureNUnit]
    public class BaseApiTest
    {
        protected ApiClient ApiClient;
        protected ILogger Logger;

        [SetUp]
        public void Setup()
        {
            ApiClient = new ApiClient();
            Logger = LoggerConfig.ConfigureLogger(TestContext.CurrentContext.Test.Name);

            AllureLifecycle.Instance.UpdateTestCase(tc =>
            {
                tc.name = TestContext.CurrentContext.Test.Name;
            });
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    Logger.Error($"Test failed: {TestContext.CurrentContext.Result.Message}");
                    if (WebDriverSingleton.Driver != null)
                    {
                        ScreenshotTaker.TakeScreenshot(WebDriverSingleton.Driver, Logger, TestContext.CurrentContext.Test.Name);
                    }
                }
            }
            finally
            {
                Log.CloseAndFlush();
                ApiClient?.Dispose();
            }
        }
    }
}