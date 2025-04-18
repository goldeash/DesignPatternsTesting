using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WebUITests.NUnit.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver _driver;
        protected readonly WebDriverWait _wait;

        protected BasePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }
    }
}