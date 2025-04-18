using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;
using WebUITests.XUnit.Utilities;

namespace WebUITests.XUnit
{
    public class BaseTest : IDisposable
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        public BaseTest()
        {
            driver = WebDriverSingleton.Driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        public void Dispose()
        {
        }
    }

    public class TestCollectionFixture : IDisposable
    {
        public TestCollectionFixture()
        {
        }

        public void Dispose()
        {
            WebDriverSingleton.QuitDriver();
        }
    }
}