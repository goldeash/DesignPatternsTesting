using OpenQA.Selenium;

namespace WebUITests.XUnit.Pages
{
    public class ContactPage : BasePage
    {
        public ContactPage(IWebDriver driver) : base(driver) { }

        public bool ContainsText(string text)
        {
            return _wait.Until(d => d.PageSource.Contains(text));
        }
    }
}