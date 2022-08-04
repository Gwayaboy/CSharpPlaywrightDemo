using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace PlaywrightTests.PageObjects;

public abstract class PageObject 
{
    protected IPage Page { get; private set; }

    public string Url => Page.Url;

    public async Task<string> Title() => await Page.TitleAsync();

    internal async static Task<TPage> GoToInitial<TPage>(string startUpUrl, IPage page) where TPage : PageObject, new()
    {
        if (page == null) throw new ApplicationException("Please provide with an instance of IPage instance to proceed");
        await page.GotoAsync(startUpUrl);
        return new TPage { Page = page };
    }

    protected TPage GoTo<TPage>(IPage? page = null)
      where TPage : PageObject, new()
    {
        return new TPage { Page = page ?? Page };
    }

}

