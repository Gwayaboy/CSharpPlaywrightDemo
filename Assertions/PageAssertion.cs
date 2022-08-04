using Microsoft.Playwright;
using PlaywrightTests.PageObjects;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaywrightTests.Assertions;

public abstract class PageAssertions<TPageObject> : IPageAssertions
    where TPageObject : PageObject, new()
{

    protected TPageObject PageObject { get; private set; }

    protected IPage Page { get; private set; }

    protected bool IsNegated { get;  private set; }

    public static TPageAssertions Create<TPageAssertions>(TPageObject pageObj, IPage page, bool isNegated = false) where TPageAssertions : new()
    {
        var assertion = new TPageAssertions();
        var pageAssertion = assertion as PageAssertions<TPageObject>;
        if (pageAssertion == null) throw new Exception($"{typeof(TPageAssertions).Name} should be a concrete instance of PageAssertion<{typeof(TPageObject).Name}>");

        pageAssertion.PageObject = pageObj;
        pageAssertion.IsNegated = isNegated;
        pageAssertion.Page = page;
        return assertion;
    }

    protected void ValidateAssertion(bool conditionIsNotFulfilled, string errorMessage)
    {
        if (IsNegated) 
        {
            conditionIsNotFulfilled = !conditionIsNotFulfilled;
            errorMessage = errorMessage.Replace("Expected", "Not expected", StringComparison.InvariantCultureIgnoreCase);
        }

        if (conditionIsNotFulfilled)
        {
            throw new PlaywrightException(errorMessage);
        }
    }

    public async Task ToHaveUrlContain(string urlSubset)
    {
        await Page.WaitForLoadStateAsync(LoadState.Load);
        var actualUrl = Page.Url;
        ValidateAssertion(!actualUrl.Contains(urlSubset, StringComparison.InvariantCultureIgnoreCase), $"Expected to have url contain \"{urlSubset}\" \n but the page url \"{actualUrl}\" did");
    }

    #region Wrapping Playwright's IPageAssertions Implementation

    public IPageAssertions Not => Microsoft.Playwright.Assertions.Expect(Page).Not;    

    public Task ToHaveTitleAsync(string titleOrRegExp, PageAssertionsToHaveTitleOptions? options = null)
    {
        return Expect(Page).ToHaveTitleAsync(titleOrRegExp, options);
    }

    public Task ToHaveTitleAsync(Regex titleOrRegExp, PageAssertionsToHaveTitleOptions? options = null)
    {
        return Expect(Page).ToHaveTitleAsync(titleOrRegExp, options);
    }

    public Task ToHaveURLAsync(string urlOrRegExp, PageAssertionsToHaveURLOptions? options = null)
    {
        return Expect(Page).ToHaveURLAsync(urlOrRegExp, options);
    }

    public Task ToHaveURLAsync(Regex urlOrRegExp, PageAssertionsToHaveURLOptions? options = null)
    {
        return Expect(Page).ToHaveURLAsync(urlOrRegExp, options);
    }

    private IPageAssertions Expect(IPage page)
    {
        var assertions = Microsoft.Playwright.Assertions.Expect(Page);

        if (IsNegated) assertions = assertions.Not;
        return assertions;
    }

    #endregion
}



