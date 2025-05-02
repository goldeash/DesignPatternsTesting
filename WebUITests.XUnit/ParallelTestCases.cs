using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;
using System.Threading;
using FluentAssertions;
using WebUITests.XUnit.Pages;
using WebUITests.XUnit.Models;
using WebUITests.XUnit.Factories;
using Serilog;
using Xunit.Abstractions;

namespace WebUITests.XUnit
{
    [Collection("Parallel")]
    [Trait("Category", "UI")]
    public class ParallelTestCases : BaseTest
    {
        private readonly HomePage _homePage;
        private readonly ContactPage _contactPage;

        public ParallelTestCases(ITestOutputHelper output) : base(output)
        {
            _homePage = new HomePage(driver);
            _contactPage = new ContactPage(driver);

            logger.Debug("Navigating to homepage");
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            logger.Information("Homepage loaded successfully");
        }

        [Fact]
        [Trait("Type", "Navigation")]
        public void TC1_VerifyAboutEhuPage()
        {
            try
            {
                logger.Information("Starting navigation test to About page");

                logger.Debug("Clicking on About page link");
                _homePage.GoToAboutPage();

                logger.Debug("Waiting for About page to load");
                wait.Until(d => d.Url.Contains("/about/"));

                logger.Debug("Getting page heading text");
                var h1Text = driver.FindElement(By.TagName("h1")).Text;
                logger.Information("Page heading text: '{H1Text}'", h1Text);

                logger.Debug("Verifying heading contains 'About'");
                h1Text.Should().Contain("About", "because the About page should contain 'About' in the heading");

                logger.Information("About page verification completed successfully");
            }
            catch (Exception ex)
            {
                LogTestFailure(ex.Message, ex.StackTrace);
                throw;
            }
        }

        [Fact]
        [Trait("Type", "Search")]
        public void TC2_VerifySearchFunctionality()
        {
            try
            {
                logger.Information("Starting search functionality test");

                logger.Debug("Opening search dialog");
                _homePage.OpenSearch();

                logger.Debug("Waiting for search input to appear");
                Thread.Sleep(500);

                logger.Debug("Finding search input element");
                var searchInput = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return document.querySelector('input[name=\"s\"]');");

                logger.Debug("Entering search term 'study programs'");
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = 'study programs';", searchInput);

                logger.Debug("Finding search button");
                var searchButton = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return document.querySelector('button[type=\"submit\"]');");

                logger.Debug("Clicking search button");
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", searchButton);

                logger.Debug("Waiting for search results page");
                wait.Until(d => d.Url.Contains("?s=study+programs"));

                logger.Debug("Finding search results");
                var results = driver.FindElements(By.CssSelector(".search-results li, .post-item"));

                logger.Information("Found {ResultCount} search results", results.Count);
                results.Should().NotBeEmpty("because the search should return at least one result");

                logger.Information("Search functionality test completed successfully");
            }
            catch (Exception ex)
            {
                LogTestFailure(ex.Message, ex.StackTrace);
                throw;
            }
        }

        [Theory]
        [InlineData("franciskscarynacr@gmail.com")]
        [InlineData("+370 68 771365")]
        [InlineData("+375 29 5781488")]
        [Trait("Type", "Contact")]
        public void TC4_VerifyContactInfo(string expectedText)
        {
            try
            {
                logger.Information("Starting contact info verification for: {ExpectedText}", expectedText);

                logger.Debug("Navigating to contact page");
                driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");

                logger.Debug("Checking if page contains text");
                var containsText = _contactPage.ContainsText(expectedText);

                logger.Information("Text '{ExpectedText}' found: {ContainsText}", expectedText, containsText);
                containsText.Should().BeTrue($"because the contact page should contain '{expectedText}'");

                logger.Information("Contact info verification completed successfully");
            }
            catch (Exception ex)
            {
                LogTestFailure(ex.Message, ex.StackTrace);
                throw;
            }
        }

        [Fact]
        [Trait("Type", "User")]
        public void TC5_TestUserBuilder()
        {
            try
            {
                logger.Information("Starting UserBuilder test");

                logger.Debug("Creating test user with builder");
                var user = new User.UserBuilder()
                    .WithUsername("testuser")
                    .WithPassword("password123")
                    .WithEmail("test@example.com")
                    .IsAdmin(false)
                    .Build();

                logger.Debug("Verifying user properties");
                logger.Information("User created - Username: {Username}, Email: {Email}, IsAdmin: {IsAdmin}",
                    user.Username, user.Email, user.IsAdmin);

                user.Username.Should().Be("testuser", "because that's the username we set");
                user.IsAdmin.Should().BeFalse("because we set IsAdmin to false");

                logger.Information("UserBuilder test completed successfully");
            }
            catch (Exception ex)
            {
                LogTestFailure(ex.Message, ex.StackTrace);
                throw;
            }
        }

        [Fact]
        [Trait("Type", "User")]
        public void TC6_TestUserFactory()
        {
            try
            {
                logger.Information("Starting UserFactory test");

                logger.Debug("Creating admin user");
                var adminUser = UserFactory.CreateAdminUser();

                logger.Debug("Creating standard user");
                var standardUser = UserFactory.CreateStandardUser();

                logger.Information("Admin user created - Username: {Username}, IsAdmin: {IsAdmin}",
                    adminUser.Username, adminUser.IsAdmin);
                logger.Information("Standard user created - Username: {Username}, IsAdmin: {IsAdmin}",
                    standardUser.Username, standardUser.IsAdmin);

                adminUser.IsAdmin.Should().BeTrue("because CreateAdminUser should create admin users");
                standardUser.IsAdmin.Should().BeFalse("because CreateStandardUser should create non-admin users");

                logger.Information("UserFactory test completed successfully");
            }
            catch (Exception ex)
            {
                LogTestFailure(ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}