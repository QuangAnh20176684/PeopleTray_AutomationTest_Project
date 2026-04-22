using Microsoft.Playwright;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace PeoTest;

public class DrillbitsPage : CommonBasePage
{
    public DrillbitsPage(IPage page) : base(page) { }

    // =========================
    // 🔹 SELECTOR
    // =========================
    private ILocator btnClearFilters => _page.Locator("//a[@id='lnkClearFilters']");
    private ILocator btnSearch => _page.Locator("//button[@id='btnSearch']");
    private ILocator filterBitNum => _page.Locator("//input[@id='BitName']");

    private ILocator filterEquipment => _page.Locator("//div[@id='s2id_EquipmentItemId']");
    private ILocator filterEquipmentInput => _page.Locator("//input[@id='s2id_autogen1_search']");
    private ILocator filterStartDateFrom => _page.Locator("//input[@id='dtStartFromDate']");
    private ILocator filterStartDateTo => _page.Locator("//input[@id='dtStartToDate']");
    private ILocator filterExpiryDateFrom => _page.Locator("//input[@id='dtEndFromDate']");
    private ILocator filterExpiryDateTo => _page.Locator("//input[@id='dtEndToDate']");

    private ILocator previousPage => _page.Locator("//a[@id='tblDrillBits_previous']");
    private ILocator nextPage => _page.Locator("//a[@id='tblDrillBits_next']");
    private ILocator drillBitTable => _page.Locator("//table[@id='tblDrillBits']");

    private ILocator processingStatus => _page.Locator("#tblDrillBits_processing");


    // =========================
    // 🔹 ACTIONS
    // =========================
    public async Task FilterBitnum(string optionName)
    {
        await filterBitNum.FillAsync(optionName);
    }

    public async Task FilterEquipment(string optionName)
    {
        await filterEquipment.ClickAsync();
        await filterEquipmentInput.FillAsync(optionName);
        await _page.WaitForTimeoutAsync(500);
        await _page.Keyboard.PressAsync("Enter");


    }
    public async Task FilterStartDate(string startDate, string endDate)
    {
        await filterStartDateFrom.FillAsync(startDate);
        await _page.Keyboard.PressAsync("Enter");
        await filterStartDateTo.FillAsync(endDate);
        await _page.Keyboard.PressAsync("Enter");
    }
    public async Task FilterExpiryDate(string startDate, string endDate)
    {
        await filterExpiryDateFrom.FillAsync(startDate);
        await _page.Keyboard.PressAsync("Enter");
        await filterExpiryDateTo.FillAsync(endDate);
        await _page.Keyboard.PressAsync("Enter");
    }
    public async Task Search()
    {
        await btnSearch.ClickAsync();

        await processingStatus
     .WaitForAsync(new()
     {
         State = WaitForSelectorState.Hidden
     });
    }
    public async Task ClearFilters()
    {
        await btnClearFilters.ClickAsync();
    }
    public async Task NextPage(string optionName)
    {
        await nextPage.ClickAsync();
    }
    public async Task PreviousPage(string optionName)
    {
        await previousPage.ClickAsync();
    }
    public async Task<List<string>> GetBitnumberList()
    {
        var bitnums = new List<string>();

        var rows = _page.Locator("//table[@id='tblDrillBits']//tbody//tr");
        var count = await rows.CountAsync();

        for (int i = 1; i <= count; i++)
        {
            var bitnum = await _page
                .Locator($"//table[@id='tblDrillBits']//tbody//tr[{i}]//td[1]")
                .InnerTextAsync();


            bitnums.Add(bitnum);
        }

        return bitnums;
    }
    public async Task<List<string>> GetEquipmentList()
    {
        var equipments = new List<string>();

        var rows = _page.Locator("//table[@id='tblDrillBits']//tbody//tr");
        var count = await rows.CountAsync();

        for (int i = 1; i <= count; i++)
        {
            var equipment = await _page
                .Locator($"//table[@id='tblDrillBits']//tbody//tr[{i}]//td[4]")
                .InnerTextAsync();


            equipments.Add(equipment);
        }

        return equipments;
    }
    public async Task<List<string>> GetStartDateList()
    {
        var startDateList = new List<string>();

        var rows = _page.Locator("//table[@id='tblDrillBits']//tbody//tr");
        var count = await rows.CountAsync();

        for (int i = 1; i <= count; i++)
        {
            var startDate = await _page
                .Locator($"//table[@id='tblDrillBits']//tbody//tr[{i}]//td[2]")
                .InnerTextAsync();


            startDateList.Add(startDate);
        }

        return startDateList;
    }
}