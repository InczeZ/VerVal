using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DatesAndStuff.Web.Tests
{
    [TestFixture]
    public class PersonPageTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private const string BaseURL = "http://localhost:5091";
        private bool acceptNextAlert = true;

        private Process? _blazorProcess;

        [OneTimeSetUp]
        public void StartBlazorServer()
        {
            var webProjectPath = Path.GetFullPath(Path.Combine(
                Assembly.GetExecutingAssembly().Location,
                "../../../../../../src/DatesAndStuff.Web/DatesAndStuff.Web.csproj"
                ));

            var webProjFolderPath = Path.GetDirectoryName(webProjectPath);

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                //Arguments = $"run --project \"{webProjectPath}\"",
                Arguments = "dotnet run --no-build",
                WorkingDirectory = webProjFolderPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            _blazorProcess = Process.Start(startInfo);

            // Wait for the app to become available
            var client = new HttpClient();
            var timeout = TimeSpan.FromSeconds(30);
            var start = DateTime.Now;

            while (DateTime.Now - start < timeout)
            {
                try
                {
                    var result = client.GetAsync(BaseURL).Result;
                    if (result.IsSuccessStatusCode)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        [OneTimeTearDown]
        public void StopBlazorServer()
        {
            if (_blazorProcess != null && !_blazorProcess.HasExited)
            {
                _blazorProcess.Kill(true);
                _blazorProcess.Dispose();
            }
        }

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
                driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        [TestCase(5000, 0.5)]
        [TestCase(5000, 10)]
        [TestCase(5000, 20)]
        [TestCase(5000, 0)]
        [TestCase(5000, 100)]
        public void Person_SalaryIncrease_ShouldIncrease(double salary, double percentage)
        {
            // Arrange
            driver.Navigate().GoToUrl(BaseURL);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.XPath("//*[@data-test='PersonPageNavigation']"))).Click();

            IWebElement FindStableElement(By locator)
            {
                return wait.Until(d =>
                {
                    try
                    {
                        var element = d.FindElement(locator);
                        return element.Displayed && element.Enabled ? element : null;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return null;
                    }
                });
            }

            var input = FindStableElement(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']"));
            input.Clear();
            input.SendKeys(percentage.ToString(CultureInfo.InvariantCulture));

            // Act
            var submitButton = FindStableElement(By.XPath("//*[@data-test='SalaryIncreaseSubmitButton']"));
            submitButton.Click();

            // Assert
            var salaryLabel = wait.Until(d =>
            {
                var element = d.FindElement(By.XPath("//*[@data-test='DisplayedSalary']"));
                return element.Text != "0" && !string.IsNullOrEmpty(element.Text) ? element : null;
            });

            var salaryAfterSubmission = double.Parse(salaryLabel.Text, CultureInfo.InvariantCulture);
            var expected = salary * (1 + percentage / 100);

            Console.WriteLine($"Salary after {percentage}% increase: {salaryAfterSubmission} (expected: {expected})");
            salaryAfterSubmission.Should().BeApproximately(expected, 0.001);
        }

        [Test]
        [TestCase(-11)]
        [TestCase(-20)]
        public void Person_SalaryIncrease_ShouldShowErrors_WhenPercentageTooLow(double percentage)
        {
            // Arrange
            driver.Navigate().GoToUrl(BaseURL);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.XPath("//*[@data-test='PersonPageNavigation']"))).Click();

            // Helper function to handle stale elements
            IWebElement FindStableElement(By locator)
            {
                return wait.Until(d =>
                {
                    try
                    {
                        var element = d.FindElement(locator);
                        return element.Displayed && element.Enabled ? element : null;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return null;
                    }
                });
            }

            // Enter invalid percentage
            var input = FindStableElement(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']"));
            input.Clear();
            input.SendKeys(percentage.ToString(CultureInfo.InvariantCulture));

            // Act - submit the form
            var submitButton = FindStableElement(By.XPath("//*[@data-test='SalaryIncreaseSubmitButton']"));
            submitButton.Click();

            // Wait for errors to appear
            var topError = wait.Until(d =>
            {
                var element = d.FindElement(By.XPath("//*[@data-test='ValidationSummary']"));
                return !string.IsNullOrWhiteSpace(element.Text) ? element : null;
            });

            var fieldError = wait.Until(d =>
            {
                var element = d.FindElement(By.XPath("//*[@data-test='SalaryIncreasePercentageError']"));
                return !string.IsNullOrWhiteSpace(element.Text) ? element : null;
            });

            // Assert - verify both error messages exist and contain text
            topError.Text.Should().NotBeNullOrWhiteSpace("Top validation summary should appear");
            fieldError.Text.Should().NotBeNullOrWhiteSpace("Field-specific error should appear");

            // Additional assertions if you know the expected error messages
            // fieldError.Text.Should().Contain("must be greater than or equal to -10");

            Console.WriteLine($"Top error message: {topError.Text}");
            Console.WriteLine($"Field error message: {fieldError.Text}");
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}