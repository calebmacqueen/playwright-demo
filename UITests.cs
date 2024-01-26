using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlaywrightTests;

[TestClass]
public class UITests : PageTest
{
    string BASE_URL = "http://was.tamgc.net/";

    [TestMethod]
    public async Task PageTitleIsDisplayed()
    {
        await Expect(Page.Locator("#title")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task NavigateToUSSMissouri()
    {
        await Page.GetByAltText("United States").ClickAsync();
        await Page.GetByText("USS Missouri").ClickAsync();
        AssertOnUSSMissouriDetails();
    }

    [TestMethod]
    public async Task SearchForUSSMissouri()
    {
        var searchBox = Page.Locator("[name='query']").First;
        await searchBox.FillAsync("USS Missouri");
        await searchBox.PressAsync("Enter");
        await Page.GetByText("USS Missouri").ClickAsync();
        AssertOnUSSMissouriDetails();
    }

    [TestMethod]
    public async Task SearchForUSSMissouriWithPartialText()
    {
        var searchBox = Page.Locator("[name='query']").First;
        await searchBox.FillAsync("Missouri");
        await searchBox.PressAsync("Enter");
        await Page.GetByText("USS Missouri").ClickAsync();
        AssertOnUSSMissouriDetails();
    }

    [TestMethod]
    public async Task FindUSSMissouriWithAdvancedSearch()
    {
        await Page.GetByText("[Advance]").ClickAsync();
        await Page.Locator("[name='nation']").SelectOptionAsync(new SelectOptionValue { Value = "United States" });
        await Page.Locator("[name='type']").SelectOptionAsync(new SelectOptionValue { Value = "Battleship" });
        await Page.Locator("[name='rarity']").SelectOptionAsync(new SelectOptionValue { Value = "Rare" });
        await Page.Locator("[name='search']").ClickAsync();
        await Page.GetByText("USS Missouri").ClickAsync();
        AssertOnUSSMissouriDetails();
    }

    [TestMethod]
    public async Task GibberishSearchFindsNothing()
    {
        var searchBox = Page.Locator("[name='query']").First;
        await searchBox.FillAsync("gaksdjf;hfA;K");
        await searchBox.PressAsync("Enter");
        await Expect(Page.GetByText("0 record(s) found.")).ToBeVisibleAsync();
    }

    [TestInitialize]
    public async Task Initialize()
    {
        await Page.GotoAsync(BASE_URL);
    }

    private async void AssertOnUSSMissouriDetails()
    {
        await Expect(Page.GetByText("USS Missouri")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Task Force - 28/60 - Rare")).ToBeVisibleAsync();
    }
}