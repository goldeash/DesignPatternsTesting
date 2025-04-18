using OpenQA.Selenium;

namespace WebUITests.XUnit.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        private IWebElement UsernameInput => _wait.Until(d => d.FindElement(By.Id("username")));
        private IWebElement PasswordInput => _wait.Until(d => d.FindElement(By.Id("password")));
        private IWebElement LoginButton => _wait.Until(d => d.FindElement(By.Id("login-btn")));

        public void LoginAs(string username, string password)
        {
            UsernameInput.SendKeys(username);
            PasswordInput.SendKeys(password);
            LoginButton.Click();
        }
    }
}