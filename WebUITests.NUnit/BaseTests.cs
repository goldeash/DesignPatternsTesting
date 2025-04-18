using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WebUITests.NUnit.Utilities;

namespace WebUITests.NUnit
{
    [Parallelizable(ParallelScope.All)]
    public class BaseTest
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = WebDriverSingleton.Driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            WebDriverSingleton.QuitDriver();
        }
    }
}