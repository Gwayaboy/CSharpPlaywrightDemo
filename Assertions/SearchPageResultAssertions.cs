using Microsoft.Playwright;
using PlaywrightTests.PageObjects;
using System.Threading.Tasks;

namespace PlaywrightTests.Assertions;

public class SearchPageResultAssertions : PageAssertion<SearchPageResult>, IPageAssertions
{

    public async Task ToHaveAtLeastNumberOfResults(long minimumNumberOfResults)
    {
        var actualNumberOfResults = await PageObject.NumberOfResults();
        if (actualNumberOfResults < minimumNumberOfResults)
        {
            throw new PlaywrightException($"Expected at least {minimumNumberOfResults} but has {actualNumberOfResults}");
        }
    }

    public async Task ToHaveSearchedText(string expectedSearchResult)
    {
        var actualSearchedText = await PageObject.SearchedText();
        if (string.Equals(expectedSearchResult, actualSearchedText, System.StringComparison.InvariantCultureIgnoreCase))
        {
            throw new PlaywrightException($"Expected the search result text to be \"{expectedSearchResult}\" but was \"{actualSearchedText}\"");
        }
    }
}



