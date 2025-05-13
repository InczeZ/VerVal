using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace WizzAir.Tests
{
    [TestFixture]
    public class FlightSearchTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private const string BaseUrl = "https://wizzair.com";

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            _driver.Navigate().GoToUrl(BaseUrl);
            AcceptCookies();
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                if (_driver != null)
                {
                    _driver.Quit();
                    _driver.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during teardown: {ex.Message}");
            }
            finally
            {
                _driver = null;
            }
        }
        [Test]
        public void FindFlightsBetweenCities()
        {
            // Click and fill origin input
            var originInput = _wait.Until(e => e.FindElement(By.XPath("//*[@data-test='search-departure-station']")));
            originInput.Click();
            originInput.SendKeys("Budapest");

            // Wait and click the correct suggestion
            var originSuggestion = _wait.Until(e => e.FindElement(By.XPath("//mark[contains(text(),'Budapest')]")));
            originSuggestion.Click();

            // Click and fill destination input
            var destInput = _wait.Until(e => e.FindElement(By.XPath("//*[@data-test='search-arrival-station']")));
            destInput.Click();
            destInput.SendKeys("Tirgu Mures");

            var destSuggestion = _wait.Until(e => e.FindElement(By.XPath("//mark[contains(text(),'Tirgu Mures')]")));
            destSuggestion.Click();

            // Set a date 7 days from now
            var date = DateTime.Today.AddDays(7);
            var dateSelector = _wait.Until(e => e.FindElement(By.CssSelector($"button[data-testid='departure-date-{date:yyyy-MM-dd}']")));
            dateSelector.Click();

            // Submit the search
            var searchButton = _wait.Until(e => e.FindElement(By.CssSelector("button[data-test='flight-search-submit']")));
            searchButton.Click();

            // Wait for results
            _wait.Until(d =>
            {
                var flights = d.FindElements(By.CssSelector(".flight-card"));
                return flights.Count >= 2;
            });
        }


        private void AcceptCookies()
        {
            try
            {
                var cookieAccept = _wait.Until(e =>
                    e.FindElement(By.Id("onetrust-accept-btn-handler")));
                cookieAccept.Click();
            }
            catch (WebDriverTimeoutException)
            {
                // Cookies already accepted or not present
            }
        }
    }
}