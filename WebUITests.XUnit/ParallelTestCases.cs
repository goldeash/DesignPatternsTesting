using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;
using System.Threading;
using WebUITests.XUnit.Pages;
using WebUITests.XUnit.Models;
using WebUITests.XUnit.Factories;

namespace WebUITests.XUnit
{
    [Collection("Parallel")]
    [Trait("Category", "UI")]
    public class ParallelTestCases : BaseTest
    {
        private readonly HomePage _homePage;
        private readonly ContactPage _contactPage;

        public ParallelTestCases()
        {
            _homePage = new HomePage(driver);
            _contactPage = new ContactPage(driver);
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
        }

        [Fact]
        [Trait("Type", "Navigation")]
        public void TC1_VerifyAboutEhuPage()
        {
            _homePage.GoToAboutPage();
            wait.Until(d => d.Url.Contains("/about/"));
            Assert.Contains("About", driver.FindElement(By.TagName("h1")).Text);
        }

        [Fact]
        [Trait("Type", "Search")]
        public void TC2_VerifySearchFunctionality()
        {
            _homePage.OpenSearch();
            Thread.Sleep(500);
            var searchInput = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return document.querySelector('input[name=\"s\"]');");
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = 'study programs';", searchInput);
            var searchButton = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return document.querySelector('button[type=\"submit\"]');");
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", searchButton);
            wait.Until(d => d.Url.Contains("?s=study+programs"));
            Assert.NotEmpty(driver.FindElements(By.CssSelector(".search-results li, .post-item")));
        }

        [Theory]
        [InlineData("franciskscarynacr@gmail.com")]
        [InlineData("+370 68 771365")]
        [InlineData("+375 29 5781488")]
        [Trait("Type", "Contact")]
        public void TC4_VerifyContactInfo(string expectedText)
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");
            Assert.True(_contactPage.ContainsText(expectedText));
        }

        [Fact]
        [Trait("Type", "User")]
        public void TC5_TestUserBuilder()
        {
            var user = new User.UserBuilder()
                .WithUsername("testuser")
                .WithPassword("password123")
                .WithEmail("test@example.com")
                .IsAdmin(false)
                .Build();

            Assert.Equal("testuser", user.Username);
            Assert.False(user.IsAdmin);
        }

        [Fact]
        [Trait("Type", "User")]
        public void TC6_TestUserFactory()
        {
            var adminUser = UserFactory.CreateAdminUser();
            var standardUser = UserFactory.CreateStandardUser();

            Assert.True(adminUser.IsAdmin);
            Assert.False(standardUser.IsAdmin);
        }
    }
}