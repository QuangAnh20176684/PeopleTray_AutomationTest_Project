using Microsoft.Playwright;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeoTest;

public class CommonBasePage
{
    protected readonly IPage _page;

    public CommonBasePage(IPage page)
    {
        _page = page;
    }

    // =========================
    // 🔹 NAVIGATION
    // =========================
    public async Task Navigate(string url)
    {
        Console.WriteLine($"Navigate to: {url}");
        await _page.GotoAsync(url);
    }

    // =========================
    // 🔹 CLICK (anti-flaky nhẹ)
    // =========================
    public async Task Click(string selector)
    {
        Console.WriteLine($"Click: {selector}");

        var locator = _page.Locator(selector);

        try
        {
            await locator.ClickAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Retry click: {selector} | {ex.Message}");
            await locator.ClickAsync(); // retry 1 lần
        }
    }

    // 👉 fallback JS (chỉ khi bất khả kháng)
    public async Task ClickForce(string selector)
    {
        Console.WriteLine($"Force click (JS): {selector}");

        var element = await _page.QuerySelectorAsync(selector);
        if (element != null)
        {
            await _page.EvaluateAsync("el => el.click()", element);
        }
    }

    // =========================
    // 🔹 INPUT
    // =========================
    public async Task Type(string selector, string value)
    {
        Console.WriteLine($"Type: {selector} = {value}");

        var locator = _page.Locator(selector);

        await locator.FillAsync("");   // clear
        await locator.FillAsync(value);
    }

    // =========================
    // 🔹 FIND (giữ locator, không trả element)
    // =========================
    public ILocator Find(string selector)
    {
        return _page.Locator(selector);
    }

    // =========================
    // 🔹 WAIT (chỉ dùng khi cần)
    // =========================
    public async Task WaitForVisible(string selector)
    {
        await _page.Locator(selector).WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible
        });
    }

    public async Task WaitForHidden(string selector)
    {
        await _page.Locator(selector).WaitForAsync(new()
        {
            State = WaitForSelectorState.Hidden
        });
    }

    public async Task WaitForPageLoad()
    {
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    // =========================
    // 🔹 WAIT BUSINESS (QUAN TRỌNG)
    // =========================
    public async Task WaitForApi(string urlPart, int status = 200)
    {
        Console.WriteLine($"Wait API: {urlPart}");

        await _page.WaitForResponseAsync(resp =>
            resp.Url.Contains(urlPart) && resp.Status == status
        );
    }

    // 👉 loading spinner (custom theo project)
    public async Task WaitForLoadingGone(string selector = ".loading")
    {
        await _page.Locator(selector).WaitForAsync(new()
        {
            State = WaitForSelectorState.Hidden
        });
    }

    // =========================
    // 🔹 CHECK
    // =========================
    public async Task<bool> IsDisplayed(string selector)
    {
        try
        {
            return await _page.Locator(selector).IsVisibleAsync();
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> IsEnabled(string selector)
    {
        return await _page.Locator(selector).IsEnabledAsync();
    }

    // =========================
    // 🔹 GET DATA
    // =========================
    public async Task<string> GetText(string selector)
    {
        return await _page.Locator(selector).InnerTextAsync();
    }

    public async Task<string> GetValue(string selector)
    {
        return await _page.Locator(selector).InputValueAsync();
    }

    // =========================
    // 🔹 SCROLL
    // =========================
    public async Task ScrollTo(string selector)
    {
        await _page.Locator(selector).ScrollIntoViewIfNeededAsync();
    }

    // =========================
    // 🔹 SCREENSHOT (debug)
    // =========================
    public async Task TakeScreenshot(string name)
    {
        var path = $"screenshots/{name}_{DateTime.Now:yyyyMMddHHmmss}.png";

        Console.WriteLine($"Screenshot: {path}");

        await _page.ScreenshotAsync(new()
        {
            Path = path
        });
    }

    // =========================
    // 🔹 UTIL
    // =========================
    public long ParseLong(string text)
    {
        return long.Parse(Regex.Replace(text, "[^0-9]", ""));
    }

    // =========================
    // 🔹 CLOSE
    // =========================
    public async Task Close()
    {
        await _page.Context.Browser.CloseAsync();
    }

    // =========================
    // 🔹 MODULE AND MENU ACTIONS
    // =========================
    // =========================
    // 🔹 SELECTORS
    // =========================
    protected ILocator SwitchModule => _page.Locator("//li[@class='button dropdown' and parent::ul[@data-original-title='Switch modules']]");

    public async Task SwitchToModule(string moduleName)
    {
        await SwitchModule.ClickAsync();
        await _page.WaitForTimeoutAsync(500);
        await _page.Locator($"//ul[@data-original-title='Switch modules']//div[@class='content']//li[contains(.,'{moduleName}')]").ClickAsync();
        await _page.WaitForTimeoutAsync(500);
    }

    public async Task SwitchToOptionsLeftMenubar(string optionName)
    {
        await _page.Locator($"//div[@class='menu-space']//a[contains(.,'{optionName}')]").ClickAsync();
        await _page.WaitForTimeoutAsync(500);
    }
    public async Task SwitchToOptionsLeftMenubar(string optionName, string subOptionName)
    {
        await _page.Locator($"//div[@class='menu-space']//a[contains(.,'{optionName}')]").ClickAsync();
        await _page.WaitForTimeoutAsync(500);
        await _page.Locator($"//div[@class='menu-space']//li[@class='parent open' and contains(.,'{optionName}')]").GetByText(subOptionName).ClickAsync();
        await _page.WaitForTimeoutAsync(500);
    }
    public async Task SwitchToOptionsUpperMenuBar(string optionName)
    {
        await _page.Locator($"//div[@id='head-nav']//li[contains(.,'{optionName}')]").ClickAsync();
        await _page.WaitForTimeoutAsync(500);
    }// Switch to sub options in upper menu bar in each module, for example: Settings 
}