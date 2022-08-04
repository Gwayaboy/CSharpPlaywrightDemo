using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using PlaywrightTests.Assertions;
using PlaywrightTests.PageObjects;
using System.Threading.Tasks;

namespace PlaywrightTests;

public class PageObjectBaseTest<TPageAssertion> : PageTest where TPageAssertion:new()
{
    public TPageAssertion Expect<TPageObject>(TPageObject pageObj)
        where TPageObject : PageObject, new()
    {
        return PageAssertions<TPageObject>.Create<TPageAssertion>(pageObj, Page) ;
    }

    public async Task<TPage> NavigateTo<TPage>(string startUpUrl) where TPage : PageObject, new()
    {
        return await PageObject.GoToInitial<TPage>(startUpUrl, Page);
    }
}



