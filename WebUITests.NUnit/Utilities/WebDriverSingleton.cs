using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace WebUITests.NUnit.Utilities
{
    public sealed class WebDriverSingleton
    {
        private static IWebDriver _driver;
        private static readonly object _lock = new object();

        private WebDriverSingleton() { }

        public static IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                {
                    lock (_lock)
                    {
                        if (_driver == null)
                        {
                            var firefoxOptions = new FirefoxOptions();
                            firefoxOptions.SetPreference("intl.accept_languages", "en-US,en");
                            _driver = new FirefoxDriver(firefoxOptions);
                            _driver.Manage().Window.Maximize();
                            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                        }
                    }
                }
                return _driver;
            }
        }

        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver = null;
            }
        }
    }
}