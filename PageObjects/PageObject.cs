using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace PlaywrightTests.PageObjects;


public abstract class PageObject
{
    internal IPage Page { get; private set; }

    public string Url => Page.Url;

    public async Task<string> Title() => await Page.TitleAsync();

    internal async static Task<TPageObject> GoToInitial<TPageObject>(string startUpUrl, IPage page) where TPageObject : PageObject, new()
    {
        if (page == null) throw new ApplicationException("Please provide with an IPage instance to proceed");
        await page.GotoAsync(startUpUrl);
        return new TPageObject { Page = page };
    }

    protected TPageObject GoTo<TPageObject>(IPage? newPage = null)
      where TPageObject : PageObject, new()
    {
        return new TPageObject { Page = newPage ?? Page };
    }
}

