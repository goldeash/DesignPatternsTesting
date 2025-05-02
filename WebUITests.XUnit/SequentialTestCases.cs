using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;
using Xunit.Abstractions;
using WebUITests.XUnit.Pages;
using Serilog;

namespace WebUITests.XUnit
{
    [Collection("NonParallel")]
    [Trait("Category", "UI")]
    public class SequentialTestCases : BaseTest
    {
        private readonly HomePage _homePage;

        public SequentialTestCases(ITestOutputHelper output) : base(output)
        {
            _homePage = new HomePage(driver);

            logger.Debug("Navigating to homepage");
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            logger.Information("Homepage loaded successfully");
        }

        [Fact]
        [Trait("Type", "Localization")]
        public void TC3_VerifyLanguageChange()
        {
            try
            {
                logger.Information("Starting language change test");

                logger.Debug("Opening language switcher");
                _homePage.ChangeLanguage();

                logger.Debug("Finding Lithuanian language option");
                var lithuanianOption = wait.Until(d => d.FindElement(By.XPath("//a[contains(@href, 'lt.ehu.lt') and (contains(text(),'lt') or contains(text(),'Lietuvių'))]")));

                logger.Debug("Clicking on Lithuanian option");
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(); arguments[0].click();", lithuanianOption);

                logger.Debug("Waiting for Lithuanian version to load");
                wait.Until(d => d.Url.StartsWith("https://lt.ehu.lt/"));

                logger.Information("Language changed to Lithuanian successfully");
            }
            catch (Exception ex)
            {
                LogTestFailure(ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}