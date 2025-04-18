using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using WebUITests.NUnit.Models;
using WebUITests.NUnit.Pages;
using WebUITests.NUnit.Factories;

namespace WebUITests.NUnit
{
    [TestFixture]
    [Category("UI")]
    public class TestCases : BaseTest
    {
        private HomePage _homePage;
        private ContactPage _contactPage;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _homePage = new HomePage(driver);
            _contactPage = new ContactPage(driver);
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
        }

        [Test]
        [Category("Navigation")]
        public void TC1_VerifyAboutEhuPage()
        {
            _homePage.GoToAboutPage();
            wait.Until(d => d.Url.Contains("/about/"));
            Assert.That(driver.FindElement(By.TagName("h1")).Text, Does.Contain("About"));
        }

        [Test]
        [Category("Search")]
        public void TC2_VerifySearchFunctionality()
        {
            _homePage.OpenSearch();
            Thread.Sleep(500);
            var searchInput = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return document.querySelector('input[name=\"s\"]');");
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = 'study programs';", searchInput);
            var searchButton = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return document.querySelector('button[type=\"submit\"]');");
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", searchButton);
            wait.Until(d => d.Url.Contains("?s=study+programs"));
            Assert.That(driver.FindElements(By.CssSelector(".search-results li, .post-item")).Count > 0);
        }

        [Test]
        [Category("Localization")]
        [NonParallelizable]
        public void TC3_VerifyLanguageChange()
        {
            _homePage.ChangeLanguage();
            var lithuanianOption = wait.Until(d => d.FindElement(By.XPath("//a[contains(@href, 'lt.ehu.lt') and (contains(text(),'lt') or contains(text(),'Lietuvių'))]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(); arguments[0].click();", lithuanianOption);
            wait.Until(d => d.Url.StartsWith("https://lt.ehu.lt/"));
        }

        [Test]
        [TestCase("franciskscarynacr@gmail.com")]
        [TestCase("+370 68 771365")]
        [TestCase("+375 29 5781488")]
        [Category("Contact")]
        public void TC4_VerifyContactInfo(string expectedText)
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");
            Assert.That(_contactPage.ContainsText(expectedText), Is.True);
        }

        [Test]
        [Category("User")]
        public void TC5_TestUserBuilder()
        {
            var user = new User.UserBuilder()
                .WithUsername("testuser")
                .WithPassword("password123")
                .WithEmail("test@example.com")
                .IsAdmin(false)
                .Build();

            Assert.That(user.Username, Is.EqualTo("testuser"));
            Assert.That(user.IsAdmin, Is.False);
        }

        [Test]
        [Category("User")]
        public void TC6_TestUserFactory()
        {
            var adminUser = UserFactory.CreateAdminUser();
            var standardUser = UserFactory.CreateStandardUser();

            Assert.That(adminUser.IsAdmin, Is.True);
            Assert.That(standardUser.IsAdmin, Is.False);
        }
    }
}