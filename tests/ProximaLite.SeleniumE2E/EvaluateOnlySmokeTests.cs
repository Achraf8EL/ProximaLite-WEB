using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace ProximaLite.SeleniumE2E;

public class EvaluateOnlySmokeTests
{
    [Fact]
    public void EvaluateOnly_ShouldCreateEvaluationHistoryRow()
    {
        var baseUrl = Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "http://localhost:5000";
        var processName = "E2E-SMOKE " + DateTime.UtcNow.ToString("yyyyMMddHHmmss");

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

        void ScrollIntoView(IWebElement el) =>
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", el);

        void ClickSafe(IWebElement el)
        {
            ScrollIntoView(el);
            try { el.Click(); }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", el);
            }
        }

        driver.Navigate().GoToUrl($"{baseUrl}/Processes");

        ClickSafe(WaitVisible(By.Id("create-process-link")));
        WaitVisible(By.Id("process-name")).SendKeys(processName);
        driver.FindElement(By.Id("process-description")).SendKeys("Evaluate-only smoke");
        ClickSafe(driver.FindElement(By.Id("create-process-btn")));

        wait.Until(d => d.FindElements(By.CssSelector("a.process-link")).Count > 0);
        var myLink = driver.FindElements(By.CssSelector("a.process-link"))
            .First(a => a.Text.Trim() == processName);
        ClickSafe(myLink);

        WaitVisible(By.Id("step-name")).SendKeys("S1");
        driver.FindElement(By.Id("step-duration")).Clear();
        driver.FindElement(By.Id("step-duration")).SendKeys("1");
        driver.FindElement(By.Id("step-yield")).Clear();
        driver.FindElement(By.Id("step-yield")).SendKeys("1.0");
        driver.FindElement(By.Id("step-cost")).Clear();
        driver.FindElement(By.Id("step-cost")).SendKeys("1.0");
        ClickSafe(driver.FindElement(By.Id("add-step-btn")));

        WaitVisible(By.Id("eval-notes")).SendKeys("SMOKE");
        ClickSafe(driver.FindElement(By.Id("evaluate-btn")));

        wait.Until(d => d.PageSource.Contains("Evaluation history"));
        Assert.Contains("SMOKE", driver.PageSource);
    }
}
