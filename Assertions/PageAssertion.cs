using Microsoft.Playwright;
using PlaywrightTests.PageObjects;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaywrightTests.Assertions;

public abstract class PageAssertion<TPage> : IPageAssertions
    where TPage : PageObject
{
    protected TPage PageObject { get; private set; }

    public static TPageAssertion Create<TPageAssertion>(TPage pageObj) where TPageAssertion : new()
    {
        var assertion = new TPageAssertion();
        var pageAssertion = assertion as PageAssertion<TPage>;
        if (pageAssertion == null) throw new Exception("Please provide with an instance of PageAssertion");

        pageAssertion.PageObject = pageObj;
        return assertion;
    }

    public IPageAssertions Not => Microsoft.Playwright.Assertions.Expect(PageObject.Page).Not;

    public Task ToHaveTitleAsync(string titleOrRegExp, PageAssertionsToHaveTitleOptions? options = null)
    {
        return Microsoft.Playwright.Assertions.Expect(PageObject.Page).ToHaveTitleAsync(titleOrRegExp, options);
    }

    public Task ToHaveTitleAsync(Regex titleOrRegExp, PageAssertionsToHaveTitleOptions? options = null)
    {
        return Microsoft.Playwright.Assertions.Expect(PageObject.Page).ToHaveTitleAsync(titleOrRegExp, options);
    }

    public Task ToHaveURLAsync(string urlOrRegExp, PageAssertionsToHaveURLOptions? options = null)
    {
        return Microsoft.Playwright.Assertions.Expect(PageObject.Page).ToHaveURLAsync(urlOrRegExp, options);
    }

    public Task ToHaveURLAsync(Regex urlOrRegExp, PageAssertionsToHaveURLOptions? options = null)
    {
        return Microsoft.Playwright.Assertions.Expect(PageObject.Page).ToHaveURLAsync(urlOrRegExp, options);
    }
}



