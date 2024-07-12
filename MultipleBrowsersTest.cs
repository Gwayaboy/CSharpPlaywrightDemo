using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightTests.PageObjects;
using System.Threading.Tasks;

namespace PlaywrightTests
{
    [TestFixture(Microsoft.Playwright.BrowserType.Chromium)]
    [TestFixture(Microsoft.Playwright.BrowserType.Firefox)]
    [TestFixture(Microsoft.Playwright.BrowserType.Webkit)]
    public class MultipleBrowsersTest : PlaywrightTest
    {
        private IBrowser _browser;
        private IBrowserContext _browserContext;
        private string _browserName;

        public MultipleBrowsersTest(string browserName)
        {
            _browserName = browserName;
        }

        [SetUp]
        public async Task Setup()
        {
            var browserType = Playwright[_browserName];
            Assert.IsNotNull(browserType, $"The requested browser ({_browserName}) could not be found - make sure your BROWSER env variable is set correctly.");

            _browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });

            _browserContext = await _browser.NewContextAsync();

        }

        [Test]
        public async Task HelloWordSeachFirstResultShouldBeHelloWordProgram()
        {
            //Arrange
            var page = await _browserContext.NewPageAsync();
            var searchPage = await PageObject.GoToInitial<SearchPage>("https://bing.com", page);

            //Act
            var resultPage = await searchPage.Search("Hello World!");

            // Assertions
            Assert.IsTrue(resultPage.Url.Contains("search?q=Hello+World%21"));
        }


    }


}
