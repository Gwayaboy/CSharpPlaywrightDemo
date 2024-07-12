using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaywrightTests.PageObjects;

public class SearchPageResult : PageObject
{

    public async Task<long> NumberOfResults()
    {
        var resultsText = await Page.Locator("#b_tween_searchResults .sb_count").TextContentAsync() ?? string.Empty;
        return long.Parse( Regex.Replace(resultsText, @"[^0-9]", ""));

    }

    public async Task<string> SearchedText() => await Page.InputValueAsync("#sb_form_q");

}


