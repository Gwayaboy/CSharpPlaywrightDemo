using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightTests.Assertions;
using PlaywrightTests.PageObjects;
using System.Threading.Tasks;

namespace PlaywrightTests;

public class BingSearchPageTest : PageObjectBaseTest<SearchPageResultAssertions>
{

    protected override BrowserTypeLaunchOptions BrowserLaunchOptions()
    {
        return new BrowserTypeLaunchOptions {Headless = false};
    }
    [Category("BingTextSearch")]
    [Test]
    public async Task EmptySearchShouldNotTriggerAnySearch()
    {
        //Arrange
        var searchPage = await NavigateTo<SearchPage>("https://bing.com");

        // Act
        var returnedPage = await searchPage.Search();

        // Assertions        
        await Expect(returnedPage).ToHaveTitleAsync("Bing"); 
        await Expect(returnedPage).Not.ToHaveUrlContain("search?q=");
    }

    [Category("BingTextSearch")]
    [Test]
    public async Task HelloWordSeachShouldReturnMoreThanOneResults()
    {
        //Arrange
        var searchPage = await NavigateTo<SearchPage>("https://bing.com");

        //Act
        var resultPage = await searchPage.Search("Hello World!");

        // Assertions
        await Expect(resultPage).ToHaveUrlContain("search?q=Hello+World%21");
        await Expect(resultPage).ToHaveSearchedText("Hello World!");
        await Expect(resultPage).ToHaveAtLeastNumberOfResults(1);

    }
    
}



