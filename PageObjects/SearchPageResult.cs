using System;
using System.Threading.Tasks;

namespace PlaywrightTests.PageObjects;

public class SearchPageResult : PageObject
{

    public async Task<long> NumberOfResults()
    {
        var text = await Page.TextContentAsync("#b_tween > span.sb_count") ?? string.Empty;
        return long.Parse(text.Replace("Results", string.Empty, StringComparison.InvariantCultureIgnoreCase).Replace(",", string.Empty));
    }

    public async Task<string> SearchedText() => await Page.InputValueAsync("#sb_form_q");

}

