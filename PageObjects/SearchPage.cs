﻿using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightTests.PageObjects;

public class SearchPage : PageObject
{

    public async Task<SearchPageResult> Search(string text = "")
    {
        

        var searchTermInput = Page.Locator("#sb_form_q");
        await searchTermInput.FillAsync(text);
        await searchTermInput.PressAsync("Enter");

        return GoTo<SearchPageResult>();
    }

}

