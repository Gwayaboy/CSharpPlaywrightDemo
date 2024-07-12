using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{

    [Test]
    public async Task LongTest()
    {

        for (var i = 0; i < 100; i++)
        {
            await Page.GotoAsync("https://playwright.dev");

            // Create a locator for the get started link.
            var getStarted = Page.GetByRole(AriaRole.Link, new() { Name = "Get started" });

            // Click the get started link.
            await getStarted.ClickAsync();

            // Expects the URL to contain "intro".
            await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
        }

    }
}