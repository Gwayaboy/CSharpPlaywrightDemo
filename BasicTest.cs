using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightTests.PageObjects;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlaywrightTests
{


    public class BasicTest : ContextTest
    {
        #region secret password
        private const string myPassword = "***********";
        #endregion

        public override BrowserNewContextOptions ContextOptions()
        {
            var someString = new string("dfsdfs");

            someString += "Hello";

            return new BrowserNewContextOptions
            {
                //Videos options must be set when creating browser context
                RecordVideoDir = "videos/" + someString,
                RecordVideoSize = new RecordVideoSize() { Width = 640, Height = 480 }

            };


        }

        [Test]
        public async Task HelloWordSeachFirstResultShouldBeHelloWordProgram()
        {
            //Arramge
            var page = await Context.NewPageAsync();
            await page.GotoAsync("https://bing.com");
            await page.GetByRole(AriaRole.Link, new() { Name = "Accept" }).ClickAsync();

            //Act
            await page.TypeAsync("#sb_form_q", "Hello World!");
            await page.PressAsync("#sb_form_q", "Enter");
            await page.ClickAsync("#search_icon");

            // Assertions
            //Assert.IsTrue(page.Url.Contains("search?q=Hello+World%21"), $"Expected the page url to contain \"search?q=Hello+World%21\" but was {page.Url}");
            Expect(page).ToHaveURLAsync("search?q=Hello+World%21");

        }

        [Test]
        public async Task HelloWordSeachFirstResultShouldBeHelloWordProgramPageObjects()
        {
            //Arrange
            var page = await Context.NewPageAsync();
            var searchPage = await PageObject.GoToInitial<SearchPage>("https://bing.com", page);

            //Act
            var returnedPage = await searchPage.Search("Hello World!");

            // Assertions
            Assert.IsTrue(returnedPage.Url.Contains("search?q=Hello+World%21"));
        }

        [Test]
        [TestCase(48.902798f, 2.304884f, "Île-de-France")]
        public async Task SetGeoLocationAndGoToMyCurrentLocationOnBingMaps(float latitude, float longitude, string expectedLocation)
        {
            //Arrange

            //Tracing before creating a new page
            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });


            //GeoLocation
            await Context.SetGeolocationAsync(new Geolocation { Longitude = longitude, Latitude = latitude });
            //granted for all test sharing the same browser context 
            await Context.GrantPermissionsAsync(new[] { "geolocation" });

            var page = await Context.NewPageAsync();

            await page.GotoAsync("https://www.bing.com/maps/");
            await page.ClickAsync("button:has-text(\"Accept\")");
            await page.ClickAsync("a.panelClose");

            //Wait for the element to be available
            var locateMeButton = page.Locator("id=LocateMeButton");
            await locateMeButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible | WaitForSelectorState.Attached });

            //Act
            await locateMeButton.ClickAsync();


            //Assert
            // Assert.IsTrue(actualLocation.Contains(await page.Locator("span[data-tag=geochainSegmentContainer]").TextContentAsync()));
            await Expect(page.Locator("span[data-tag=geochainSegmentContainer]")).ToContainTextAsync(expectedLocation);

            //Revoke all permissions:
            await Context.ClearPermissionsAsync();
            await Context.Tracing.StopAsync(new TracingStopOptions { Path = "traceGeo.zip" });
        }

        [Test]
        public async Task DownloadMicrosoftEdgeBrowserForWindows11()
        {
            //Arrange
            var page = await Context.NewPageAsync();
            await page.GotoAsync("https://www.microsoft.com/en-us/edge?r=1");

            // Start the task of waiting for the download
            var waitForDownloadTask = page.WaitForDownloadAsync();

            await page.ClickAsync("a[data-bi-name=\"download edge\"]");
            await page.ClickAsync("div.combo-option ul li[name=win11]");

            //Act
            // Perform the action that initiates download
            await page.ClickAsync("a:has-text(\"Accept and download\")");
            // Wait for the download process to complete
            var download = await waitForDownloadTask;

            //Assert
            var path = await download.PathAsync();
            Assert.IsTrue(File.Exists(path));
        }


        [Test]
        public async Task UploadFileFromBuffer()
        {
            //Arrange
            var page = await Context.NewPageAsync();
            await page.GotoAsync("https://cgi-lib.berkeley.edu/ex/fup.html");

            await page.SetInputFilesAsync("form input[type=file]", new FilePayload
            {
                Name = "file.txt",
                MimeType = "text/plain",
                Buffer = System.Text.Encoding.UTF8.GetBytes("this is a test"),
            });

            //Act
            await page.ClickAsync("input[type=submit]");
            // Wait for the page to refresh complete
            await page.Locator("h1:has-text(\"File Upload Results\")").WaitForAsync();

            //Assert
            var fileContent = await page.Locator("pre").TextContentAsync();
            Assert.IsTrue(fileContent.Contains("this is a test"));
        }

        [Test]
        public async Task AddingAContributorToOneOfMyRepositoriesMultiUserScenario()
        {
            // Create a Chromium browser instance
            await using var myGitHubContext = await Browser.NewContextAsync();
            var myGitHubPage = await myGitHubContext.NewPageAsync();


            //Sign in
            await myGitHubPage.GotoAsync("https://github.com/Gwayaboy/Module2-UIAutomationTesting");
            await myGitHubPage.Locator("text=Sign in").ClickAsync();
            await myGitHubPage.Locator("input[name=\"login\"]").FillAsync("Gwayaboy");
            await myGitHubPage.Locator("input[name=\"password\"]").FillAsync(myPassword);
            await myGitHubPage.Locator("input:has-text(\"Sign in\")").ClickAsync();

            //add other github user ass contributor
            await myGitHubPage.Locator("#settings-tab").ClickAsync();
            await myGitHubPage.Locator("span:has-text(\"Collaborators\")").ClickAsync();
            await myGitHubPage.Locator("summary:has-text(\"Add people\")").ClickAsync();
            await myGitHubPage.Locator("[placeholder=\"Search by username\\, full name\\, or email\"]").FillAsync("GwayaboyDoubleGanger");
            await myGitHubPage.Locator("[aria-label=\"results\"] div:has-text(\"GwayaboyDoubleGanger Invite collaborator\")").First.ClickAsync();
            await myGitHubPage.Locator("text=Add GwayaboyDoubleGanger to this repository").ClickAsync();

            // Sign as other colloborator
            await using var anotherGitHubContext = await Browser.NewContextAsync();
            var anotherGitHubPage = await anotherGitHubContext.NewPageAsync();
            await anotherGitHubPage.GotoAsync("https://github.com/Gwayaboy/Module2-UIAutomationTesting");
            await anotherGitHubPage.Locator("text=Sign in").ClickAsync();
            await anotherGitHubPage.Locator("input[name=\"login\"]").FillAsync("GwayaboyDoubleGanger");
            await anotherGitHubPage.Locator("input[name=\"password\"]").FillAsync(myPassword);
            await anotherGitHubPage.Locator("input:has-text(\"Sign in\")").ClickAsync();

            //Accept invitation
            await anotherGitHubPage.Locator("text=View invitation").ClickAsync();
            await anotherGitHubPage.Locator("text=Accept invitation").ClickAsync();

            await Expect(anotherGitHubPage).ToHaveURLAsync("https://github.com/Gwayaboy/Module2-UIAutomationTesting");
            await Expect(anotherGitHubPage.Locator("#js-flash-container div.flash-notice")).ToHaveTextAsync("You now have push access to the Gwayaboy/Module2-UIAutomationTesting repository.");
        }

    }


}
