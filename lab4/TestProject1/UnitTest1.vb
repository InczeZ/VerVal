Imports Microsoft.VisualStudio.TestPlatform.ObjectModel

Using System;
Using FluentAssertions;
Using NUnit.Framework;
Using OpenQA.Selenium;
Using OpenQA.Selenium.Chrome;
Using OpenQA.Selenium.Support.UI;
Using System.Globalization;

Namespace WizzAir.Tests
{
    [TestFixture]
    Public Class FlightSearchTests
    {
        Private IWebDriver _driver;
        Private WebDriverWait _wait;

        [SetUp]
        Public void Setup()
        {
            _driver = New ChromeDriver();
            _wait = New WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            _driver.Manage().Window.Maximize();
        }

        [TearDown]
        Public void Teardown()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Test]
        Public void CheckTwoFlightsNextWeekBetweenMarosvasarhelyAndBudapest()
        {
            _driver.Navigate().GoToUrl("https://wizzair.com");

            AcceptCookies();
            SelectOneWayTrip();
            SetAirports("Târgu Mure?", "Budapest");
            
            var (startDate, endDate) = CalculateNextWeekDates();
            int totalFlights = CountFlightsForDateRange(startDate, endDate);

            totalFlights.Should().BeGreaterOrEqualTo(2);
        }

        Private void AcceptCookies()
        {
            Try
            {
                var cookieAccept = _wait.Until(e >= e.FindElement(By.CssSelector("button[data-test='cookie-policy-accept']")));
                cookieAccept.Click();
            }
            Catch (WebDriverTimeoutException) { }
        }

        Private void SelectOneWayTrip()
        {
            var oneWayButton = _wait.Until(e >= e.FindElement(By.CssSelector("label[data-test='search-type-oneway']")));
            oneWayButton.Click();
        }

        Private void SetAirports(String departure, String arrival)
        {
            SetAirport("departure", departure);
            SetAirport("arrival", arrival);
        }

        Private void SetAirport(String type, String city)
        {
            var input = _wait.Until(e >= e.FindElement(By.CssSelector($"input[data-test='search-{Type}-airport']")));
            input.Clear();
            input.SendKeys(city);

            var airportOption = _wait.Until(e >= e.FindElement(By.XPath($"//div[contains(@class, 'airport-list__item') and contains(., '{city}')]")));
            airportOption.Click();
        }

        Private (DateTime startDate, DateTime endDate) CalculateNextWeekDates()
        {
            DateTime today = DateTime.Today;
            int daysToMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            DateTime startDate = today.AddDays(daysToMonday);
            DateTime endDate = startDate.AddDays(6);
            Return (startDate, endDate);
        }

        Private int CountFlightsForDateRange(DateTime startDate, DateTime endDate)
        {
            int totalFlights = 0;

            For (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                SetDate(date);
                totalFlights += GetFlightsCount();
                _driver.Navigate().Back();
                WaitForSearchPage();
            }

            Return totalFlights;
        }

        Private void SetDate(DateTime Date)
        {
            var dateInput = _wait.Until(e >= e.FindElement(By.CssSelector("input[data-test='search-departure-date-input']")));
            dateInput.Clear();
            dateInput.SendKeys(date.ToString("dd/MM/yyyy"));
            
            var searchButton = _wait.Until(e >= e.FindElement(By.CssSelector("button[data-test='flight-search-submit']")));
            searchButton.Click();
        }

        Private int GetFlightsCount()
        {
            Try
            {
                var flightElements = _wait.Until(d >= d.FindElements(By.CssSelector("[data-test='flight-card']")));
                Return flightElements.Count;
            }
            Catch (WebDriverTimeoutException)
            {
                Return 0;
            }
        }

        Private void WaitForSearchPage()
        {
            _wait.Until(d => d.FindElement(By.CssSelector("input[data-test='search-departure-airport']")));
        }
    }
}