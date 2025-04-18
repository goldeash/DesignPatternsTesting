using OpenQA.Selenium;

namespace WebUITests.XUnit.Pages
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver) { }

        private IWebElement AboutLink => _wait.Until(d => d.FindElement(By.XPath("//a[contains(text(),'About') or contains(text(),'Apie')]")));
        private IWebElement SearchIcon => _wait.Until(d => d.FindElement(By.CssSelector(".header-search__link")));
        private IWebElement LanguageSwitcher => _wait.Until(d => d.FindElement(By.CssSelector(".language-switcher > li")));

        public void GoToAboutPage()
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", AboutLink);
        }

        public void OpenSearch()
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].style.display='block'; arguments[0].style.visibility='visible'; arguments[0].click();", SearchIcon);
        }

        public void ChangeLanguage()
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].style.visibility='visible'; arguments[0].click();", LanguageSwitcher);
        }
    }
}