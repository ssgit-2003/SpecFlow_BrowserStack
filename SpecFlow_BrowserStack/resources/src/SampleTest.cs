using TechTalk.SpecFlow;
using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using SeleniumExtras.WaitHelpers;
using System.Threading;

namespace SpecFlowBrowserStack
{
	[Binding]
	public class SampleTest

	{
		private readonly IWebDriver _driver;
		readonly WebDriverWait wait;

		public SampleTest()
		{
			_driver = BrowserStackSpecFlowTest.ThreadLocalDriver.Value;
			wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
		}

        [Given(@"the user navigates to ""(.*)""")]
        public void GivenTheUserNavigatesTo(string url)
        {
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(url);
		}

        [Given(@"the user logs in with username ""(.*)"" and password ""(.*)""")]
        public void ThenTheUserLogsInWithUsernameAndPassword(string username, string password)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("signin")));
            var signIn = _driver.FindElement(By.Id("signin"));

            signIn.Click();
            Thread.Sleep(2000);

            var userNameInput = _driver.FindElement(By.Id("react-select-2-input"));
            var passwordInput = _driver.FindElement(By.Id("react-select-3-input"));
            var loginButton = _driver.FindElement(By.Id("login-btn"));

            userNameInput.SendKeys(username);
			userNameInput.SendKeys(Keys.Enter);
            Thread.Sleep(1000);
            //passwordInput.Click();
            passwordInput.SendKeys(password);
            passwordInput.SendKeys(Keys.ArrowDown);  // Moves to the first suggestion
            passwordInput.SendKeys(Keys.Enter);  // Selects the suggestion
            Thread.Sleep(1000);
            loginButton.Click();
        }

        [When(@"the user filters the product view to show ""(.*)"" devices only")]
        public void WhenTheUserFiltersTheProductViewToShowDevicesOnly(string productFilter)

        {
            string filter = $"//span[@class='checkmark' and text()='{productFilter}']";
            var filterElement = _driver.FindElement(By.XPath(filter));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(filter)));
            filterElement.Click();
        }

        [When(@"the user favorites the ""(.*)"" device by clicking the yellow heart icon")]
        public void WhenTheUserFavoritesTheDeviceByClickingTheYellowHeartIcon(string model)

        {
            var favIcon = _driver.FindElement(By.XPath($"//div[@class='shelf-item']//p[@class='shelf-item__title' and text()='{model}']/parent::div/div[@class = 'shelf-stopper']//button"));
            favIcon.Click();
        }

        [Then(@"the user verifies that the ""(.*)"" device is listed on the Favorites page")]
        public void ThenTheUserVerifiesThatTheDeviceIsListedOnTheFavoritesPage(string model)

        {
            _driver.FindElement(By.Id("favourites")).Click();
            var favProduct = _driver.FindElement(By.XPath($"//p[@class='shelf-item__title' and text()='{model}']"));
            if (favProduct == null)
            {
                throw new Exception($"The model '{model}' was not found on the Favorites page.");
            }
            Assert.That(favProduct.Text.Contains(model));
        }
	}
}
