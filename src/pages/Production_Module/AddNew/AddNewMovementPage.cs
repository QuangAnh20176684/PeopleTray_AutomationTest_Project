using Microsoft.Playwright;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeoTest;

public class AddNewMovementPage : CommonBasePage
{
    public AddNewMovementPage(IPage page) : base(page) { }

    // =========================
    // 🔹 SELECTOR
    // =========================

    private ILocator formAddNewMovement => _page.Locator("#frmNewMaterialMovement");


    // =========================
    // 🔹 ACTIONS
    // =========================

    public async Task selectMovementType(string optionName)
    {
        await formAddNewMovement.Locator("#select2-chosen-13").ClickAsync();
        await _page.Locator("#s2id_autogen13_search").FillAsync(optionName);
        await _page.WaitForTimeoutAsync(500);
        await _page.Keyboard.PressAsync("Enter");

    }


    public async Task selectSite(string optionName)
    {
        await formAddNewMovement.Locator("#select2-chosen-12").ClickAsync();
        await _page.Locator("#s2id_autogen12_search").FillAsync(optionName);
        await _page.WaitForTimeoutAsync(500);
        await _page.Keyboard.PressAsync("Enter");

    }

}