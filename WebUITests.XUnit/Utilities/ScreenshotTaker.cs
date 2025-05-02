using OpenQA.Selenium;
using Serilog;

namespace WebUITests.XUnit.Utilities
{
    public static class ScreenshotTaker
    {
        public static void TakeScreenshot(IWebDriver driver, ILogger logger, string testName)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{testName}.png";
                var filePath = Path.Combine("logs", "screenshots", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                screenshot.SaveAsFile(filePath);

                logger.Information("Screenshot saved to: {FilePath}", filePath);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to take screenshot");
            }
        }
    }
}