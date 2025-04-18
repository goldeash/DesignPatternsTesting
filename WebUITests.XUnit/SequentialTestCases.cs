using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;
using WebUITests.XUnit.Pages;

namespace WebUITests.XUnit
{
    [Collection("NonParallel")]
    [Trait("Category", "UI")]
    public class SequentialTestCases : BaseTest
    {
        private readonly HomePage _homePage;

        public SequentialTestCases()
        {
            _homePage = new HomePage(driver);
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
        }

        [Fact]
        [Trait("Type", "Localization")]
        public void TC3_VerifyLanguageChange()
        {
            _homePage.ChangeLanguage();
            var lithuanianOption = wait.Until(d => d.FindElement(By.XPath("//a[contains(@href, 'lt.ehu.lt') and (contains(text(),'lt') or contains(text(),'Lietuvių'))]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(); arguments[0].click();", lithuanianOption);
            wait.Until(d => d.Url.StartsWith("https://lt.ehu.lt/"));
        }
    }
}