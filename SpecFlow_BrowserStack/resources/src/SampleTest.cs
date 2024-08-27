using TechTalk.SpecFlow;
using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using SeleniumExtras.WaitHelpers;
using System.Threading;
using SpecFlow_BrowserStack.resources.src;

namespace SpecFlowBrowserStack
{
	[Binding]
	public class SampleTest

	{
		private readonly IWebDriver _driver;
		readonly WebDriverWait wait;
        public BStackDemoPOM bStackDemoPOM;

        public SampleTest()
		{
			_driver = BrowserStackSpecFlowTest.ThreadLocalDriver.Value;
			wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            bStackDemoPOM = new BStackDemoPOM(_driver);
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
            bStackDemoPOM.SignIn(username, password);
        }

        [When(@"the user filters the product view to show ""(.*)"" devices only")]
        public void WhenTheUserFiltersTheProductViewToShowDevicesOnly(string productFilter)

        {
            bStackDemoPOM.SetFilter(productFilter);
        }

        [When(@"the user favorites the ""(.*)"" device by clicking the yellow heart icon")]
        public void WhenTheUserFavoritesTheDeviceByClickingTheYellowHeartIcon(string model)

        {
          bStackDemoPOM.SetFavourite(model);
        }

        [Then(@"the user verifies that the ""(.*)"" device is listed on the Favorites page")]
        public void ThenTheUserVerifiesThatTheDeviceIsListedOnTheFavoritesPage(string model)

        {
            bStackDemoPOM.NavigateToFavourites();
            string favProduct = bStackDemoPOM.GetFavourite(model);
            Assert.That(favProduct.Contains(model));
        }
	}
}
