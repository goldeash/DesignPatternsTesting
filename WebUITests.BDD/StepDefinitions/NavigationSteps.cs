using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using WebUITests.NUnit.Pages;
using WebUITests.NUnit.Utilities;

namespace WebUITests.BDD.StepDefinitions
{
    [Binding]
    public class NavigationSteps
    {
        private readonly IWebDriver _driver;
        private readonly HomePage _homePage;

        public NavigationSteps()
        {
            _driver = WebDriverSingleton.Driver;
            _homePage = new HomePage(_driver);
        }

        [Given(@"I am on the homepage")]
        public void GivenIAmOnTheHomepage()
        {
            _driver.Navigate().GoToUrl("https://en.ehu.lt/");
        }

        [When(@"I click on the About link")]
        public void WhenIClickOnTheAboutLink()
        {
            _homePage.GoToAboutPage();
        }

        [Then(@"I should be on the About page")]
        public void ThenIShouldBeOnTheAboutPage()
        {
            Assert.That(_driver.Url.Contains("/about/"), Is.True);
        }

        [Then(@"the page should contain ""(.*)"" in the heading")]
        public void ThenThePageShouldContainInTheHeading(string text)
        {
            Assert.That(_driver.FindElement(By.TagName("h1")).Text, Does.Contain(text));
        }

        [When(@"I open the search dialog")]
        public void WhenIOpenTheSearchDialog()
        {
            _homePage.OpenSearch();
        }

        [When(@"I search for ""(.*)""")]
        public void WhenISearchFor(string searchTerm)
        {
            var searchInput = (IWebElement)((IJavaScriptExecutor)_driver).ExecuteScript("return document.querySelector('input[name=\"s\"]');");
            ((IJavaScriptExecutor)_driver).ExecuteScript($"arguments[0].value = '{searchTerm}';", searchInput);
            var searchButton = (IWebElement)((IJavaScriptExecutor)_driver).ExecuteScript("return document.querySelector('button[type=\"submit\"]');");
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", searchButton);
        }

        [Then(@"I should see search results")]
        public void ThenIShouldSeeSearchResults()
        {
            Assert.That(_driver.FindElements(By.CssSelector(".search-results li, .post-item")).Count, Is.GreaterThan(0));
        }

        [Given(@"I am on the English version homepage")]
        public void GivenIAmOnTheEnglishVersionHomepage()
        {
            _driver.Navigate().GoToUrl("https://en.ehu.lt/");
        }

        [When(@"I change the language to Lithuanian")]
        public void WhenIChangeTheLanguageToLithuanian()
        {
            _homePage.ChangeLanguage();
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            var lithuanianOption = wait.Until(d => d.FindElement(By.XPath("//a[contains(@href, 'lt.ehu.lt') and (contains(text(),'lt') or contains(text(),'Lietuvių'))]")));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(); arguments[0].click();", lithuanianOption);
        }

        [Then(@"I should be on the Lithuanian version homepage")]
        public void ThenIShouldBeOnTheLithuanianVersionHomepage()
        {
            Assert.That(_driver.Url.StartsWith("https://lt.ehu.lt/"), Is.True);
        }
    }
}