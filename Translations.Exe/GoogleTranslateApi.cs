using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace TranslatorStore
{
    public class GoogleTranslateApi : IDisposable
    {
        public GoogleTranslateApi()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://translate.google.com/?hl=nl");

            waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
        }

        private ChromeDriver driver;
        private WebDriverWait waiter;

        public string LatestResult { get; private set; }

        private IWebElement inputBox => driver.FindElementById("source");
        private IWebElement submitButton => driver.FindElementById("gt-submit");
        private IWebElement resultBox => driver.FindElementById("gt-res-dir-ctr");

        public void SetInput(string input)
        {
            LatestResult = resultBox.Text;
            inputBox.SendKeys(input);
        }

        public void UpdateTranslation()
        {
            waiter.Until(d => LatestResult != resultBox.Text);
            LatestResult = resultBox.Text;
        }

        public void Clear()
        {
            inputBox.Clear();
            waiter.Until(d => String.IsNullOrEmpty(resultBox.Text));
        }

        public void Dispose()
        {
            driver.Quit();
        }
    }
}
