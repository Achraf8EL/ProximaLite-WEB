using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ProximaLite.SeleniumE2E;

public class SmokeProcessFlowTests
{
    [Fact]
    public void CreateProcess_AddStep_Evaluate_ShouldShowHistoryRow()
    {
        var baseUrl = Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "http://localhost:5000";
        var processName = "E2E " + DateTime.UtcNow.ToString("yyyyMMddHHmmss");

        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--window-size=1400,900"); 
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-gpu");

        using var driver = new ChromeDriver(options);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        IWebElement WaitVisible(By by) => wait.Until(d =>
        {
            var el = d.FindElement(by);
            return el.Displayed ? el : null;
        })!;

        void ScrollIntoView(IWebElement el)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", el);
        }

        void ClickSafe(IWebElement el)
        {
            ScrollIntoView(el);

            try
            {
                el.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", el);
            }
        }

        driver.Navigate().GoToUrl($"{baseUrl}/Processes");

        var createLink = WaitVisible(By.Id("create-process-link"));
        ClickSafe(createLink);

        WaitVisible(By.Id("process-name")).SendKeys(processName);
        driver.FindElement(By.Id("process-description")).SendKeys("Created by Selenium E2E");
        ClickSafe(driver.FindElement(By.Id("create-process-btn")));

        wait.Until(d => d.FindElements(By.CssSelector("a.process-link")).Count > 0);
        var links = driver.FindElements(By.CssSelector("a.process-link"));
        var myLink = links.FirstOrDefault(a => a.Text.Trim() == processName);
        Assert.NotNull(myLink);
        ClickSafe(myLink!);

        WaitVisible(By.Id("step-name")).SendKeys("E2E Step 1");
        driver.FindElement(By.Id("step-duration")).Clear();
        driver.FindElement(By.Id("step-duration")).SendKeys("3");
        driver.FindElement(By.Id("step-yield")).Clear();
        driver.FindElement(By.Id("step-yield")).SendKeys("0.9");
        driver.FindElement(By.Id("step-cost")).Clear();
        driver.FindElement(By.Id("step-cost")).SendKeys("12.5");
        ClickSafe(driver.FindElement(By.Id("add-step-btn")));

        WaitVisible(By.Id("eval-notes")).SendKeys("E2E");
        ClickSafe(driver.FindElement(By.Id("evaluate-btn")));

        wait.Until(d => d.PageSource.Contains("Evaluation history"));
        Assert.Contains("E2E", driver.PageSource);
    }
}
