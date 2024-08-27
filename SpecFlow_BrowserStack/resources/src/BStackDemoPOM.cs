using BrowserStackSDK.Accessibility;
using NLog.Filters;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow_BrowserStack.resources.src
{
    public class BStackDemoPOM
    {
        private readonly IWebDriver webDriver;
        readonly WebDriverWait wait;
        // Properties for Web Elements

        private IWebElement _Signin => webDriver.FindElement(By.Id("signin"));
        private IWebElement UserNameInput => webDriver.FindElement(By.Id("react-select-2-input"));
        private IWebElement PasswordInput => webDriver.FindElement(By.Id("react-select-3-input"));
        private IWebElement LoginButton => webDriver.FindElement(By.Id("login-btn"));
        private IWebElement _Logout => webDriver.FindElement(By.Id("logout"));
        private IWebElement FavLink => webDriver.FindElement(By.Id("favourites"));
        private IWebElement ProductFoundMessage => webDriver.FindElement(By.XPath("//div[@class='shelf-container-header']//span"));

        private IWebElement Filter { get; set; }
        public IWebElement FavIcon { get; private set; }
        private IWebElement FavProduct { get; set; }

        // Constructor
        public BStackDemoPOM(IWebDriver driver)
        {
            this.webDriver = driver;
            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
        }


        // Actions

        // Method to set the filter based on the test value of filter for the products
        private void SetFilterByText(string filterText)
        {
            string dynamicXPath = $"//span[@class='checkmark' and text()='{filterText}']";
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(dynamicXPath)));
            Filter = webDriver.FindElement(By.XPath(dynamicXPath));
        }

        // Method to select favorite based on the test value of the model
        private void SetFavouriteByModel(string modelName)
        {
            string dynamicXPath = $"//div[@class='shelf-item']//p[@class='shelf-item__title' and text()='{modelName}']/parent::div/div[@class = 'shelf-stopper']//button";
            FavIcon = webDriver.FindElement(By.XPath(dynamicXPath));
        }

        public void SignIn(string username, string password)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("signin")));
            _Signin.Click();
            EnterUserName(username);
            EnterPassword(password);
            ClickLoginButton();
        }

        private void EnterUserName(string username)
        {
            UserNameInput.SendKeys(username);
            UserNameInput.SendKeys(Keys.Return);
        }

        private void EnterPassword(string password)
        {
            PasswordInput.SendKeys(password);
            PasswordInput.SendKeys(Keys.Return);
        }

        private void ClickLoginButton()
        {
            LoginButton.Click();
        }

        public void SetFilter(string filterText)
        {
            SetFilterByText(filterText);
            Filter.Click();
        }

        public void SetFavourite(string modelName)
        {
            SetFavouriteByModel(modelName);
            FavIcon.Click();
        }

        public void NavigateToFavourites()
        {
            FavLink.Click();
        }

        public string GetFavouriteProductFoundText()
        {
            return ProductFoundMessage.Text;
        }

        // Method to get the favorite product in the favorites list
        public string GetFavourite(string modelName)
        {
            string dynamicXPath = $"//p[@class='shelf-item__title' and text()='{modelName}']";
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(dynamicXPath)));
            FavProduct = webDriver.FindElement(By.XPath(dynamicXPath));
            return FavProduct.Text;
        }

        public void Logout()
        {
            _Logout.Click();
        }
    }
}
