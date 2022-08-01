using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using PlaywrightTests.Assertions;
using PlaywrightTests.PageObjects;
using System.Threading.Tasks;

namespace PlaywrightTests;

public class PageObjectBaseTest<TPageAssertion> : PageTest where TPageAssertion:new()
{
    public TPageAssertion Expect<TPage>(TPage pageObj)
        where TPage : PageObject
    {
        return PageAssertion<TPage>.Create<TPageAssertion>(pageObj);
    }
    public async Task<TPage> NavigateTo<TPage>(string startUpUrl, IPage page) where TPage : PageObject, new()
    {
        return await PageObject.GoToInitial<TPage>(startUpUrl, page);
    }
}



