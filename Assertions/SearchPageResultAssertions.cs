using Microsoft.Playwright;
using PlaywrightTests.PageObjects;
using System;
using System.Threading.Tasks;

namespace PlaywrightTests.Assertions;

public class SearchPageResultAssertions : PageAssertions<SearchPageResult>, IPageAssertions
{

    public new SearchPageResultAssertions Not => Create<SearchPageResultAssertions>(PageObject, Page, true);


    public async Task ToHaveAtLeastNumberOfResults(long minimumNumberOfResults)
    {
        
        var actualNumberOfResults = await PageObject.NumberOfResults();

        ValidateAssertion(actualNumberOfResults < minimumNumberOfResults, $"Expected to have at least {minimumNumberOfResults} results \nbut found {actualNumberOfResults}");
    }

   

    public async Task ToHaveSearchedText(string expectedSearchResult)
    {
        var actualSearchedText = await PageObject.SearchedText();

        var doesNotToHaveSearchedText = !(string.Equals(expectedSearchResult, actualSearchedText, StringComparison.InvariantCultureIgnoreCase));

        ValidateAssertion(doesNotToHaveSearchedText, $"Expected the search result text to be \"{expectedSearchResult}\" \nbus was \"{actualSearchedText}\"");
    }
}



