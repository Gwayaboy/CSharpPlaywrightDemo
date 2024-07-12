using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright.TestAdapter;
using PlaywrightTests.Assertions;
using PlaywrightTests.PageObjects;

namespace PlaywrightTests;

public class PageObjectBaseTest<TPageAssertion> : PlaywrightTest where TPageAssertion:new()
{
    #region added code for flexibility with browser launch options instead of inheriting from PageTest directly
    public IPage Page { get; protected set; }
    public IBrowser Browser { get; protected set; }

    protected virtual IBrowserType SetUpBrowserType()
    {
        return base.BrowserType;
    }


    private readonly List<IBrowserContext> _contexts = new();

    [SetUp]
    public async Task PageSetup()
    {
        Browser = await SetUpBrowserType().LaunchAsync(BrowserLaunchOptions());
        var context = await NewContext(ContextOptions());
        Page = await context.NewPageAsync().ConfigureAwait(continueOnCapturedContext: false);
    }

    [TearDown]
    public async Task BrowserTearDown()
    {
        if (TestOk())
        {
            foreach (var context in _contexts)
            {
                await context.CloseAsync().ConfigureAwait(false);
            }
        }
        _contexts.Clear();
        Browser = null!;
        Page = null!;
    }

    
    protected async Task<IBrowserContext> NewContext(BrowserNewContextOptions? options = null)
    {
        var context = await Browser.NewContextAsync(options).ConfigureAwait(false);
        _contexts.Add(context);
        return context;
    }

     protected virtual BrowserTypeLaunchOptions BrowserLaunchOptions()
    {
        return PlaywrightSettingsProvider.LaunchOptions;
    }

    protected virtual BrowserNewContextOptions ContextOptions()
    {
        return new()
        {
            Locale = "en-GB",
            ColorScheme = ColorScheme.Light,
        };
    }

    #endregion

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



