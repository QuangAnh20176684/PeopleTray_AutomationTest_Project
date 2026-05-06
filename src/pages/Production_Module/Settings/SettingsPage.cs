using Microsoft.Playwright;
using Microsoft.VisualBasic;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeoTest;

public class SettingsPage : CommonBasePage
{
    public SettingsPage(IPage page) : base(page) { }

    // =========================
    // 🔹 LOCATORs
    // =========================
    private ILocator addStockItemPanel => _page.Locator("//div[@id='divConsumableDetails']");
    private ILocator btnSearch => _page.Locator("//button[@id='btnSearch']");
    private ILocator processingStatus => _page.Locator("#tblConsumable_processing");
    // =========================
    // 🔹 ACTIONS
    // =========================


    public async Task SwitchToOptions(string optionName, string subOptionName)
    {
        await _page.Locator($"//div[@class='content' and contains(.,'Settings')]//label[contains(.,'{optionName}')]").ClickAsync();
        await _page.WaitForTimeoutAsync(1000);
        await _page.Locator($"//div[@class='content' and contains(.,'Settings')]//label[contains(.,'{optionName}')]/following-sibling::ul//li[contains(.,'{subOptionName}')]").ClickAsync();

    }// Switch to sub options in left menu bar in each module, for example: Settings -> Production Settings, then subOptionName
}