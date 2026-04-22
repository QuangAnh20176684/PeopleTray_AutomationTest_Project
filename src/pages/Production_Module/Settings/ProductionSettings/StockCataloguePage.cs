using Microsoft.Playwright;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PeoTest;

public class StockCatalogueTestData
{
    public string Category { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public string Cost { get; set; }
    public string PartNum { get; set; }
    public string QRcode { get; set; }
    public string Description { get; set; }
    public string SubType { get; set; }

}
public class StockCataloguePage : CommonBasePage
{
    public StockCataloguePage(IPage page) : base(page) { }

    // =========================
    // 🔹 LOCATORs
    // =========================
    private ILocator addStockItemPanel => _page.Locator("//div[@id='divConsumableDetails']");
    private ILocator btnSearch => _page.Locator("//button[@id='btnSearch']");
    private ILocator processingStatus => _page.Locator("#tblConsumable_processing");
    private ILocator resultTable => _page.Locator("#tblConsumable");
    private ILocator editStockItemPanel => _page.Locator("//div[@id='divConsumableDetails']");
    private ILocator btnClear => _page.Locator("//a[@id='lnkClearFilters']");
    // =========================
    // 🔹 ACTIONS
    // =========================


    public async Task SwitchToOptions(string optionName, string subOptionName)
    {
        await _page.Locator($"//div[@class='content' and contains(.,'Settings')]//label[contains(.,'{optionName}')]").ClickAsync();
        await _page.WaitForTimeoutAsync(1000);
        await _page.Locator($"//div[@class='content' and contains(.,'Settings')]//label[contains(.,'{optionName}')]/following-sibling::ul//li[contains(.,'{subOptionName}')]").ClickAsync();

    }// Switch to sub options in left menu bar in each module, for example: Settings -> Production Settings, then subOptionName
    public async Task addStockItem(string category, string name, string unit, string cost, string partNum, string QRcode, string desc, bool isActive)
    {
        await _page.Locator("//a[@id='btnAddConsumable']").ClickAsync();
        if (isActive == false)
        {
            await addStockItemPanel.Locator("//input[@id='IsActive']").ClickAsync();

        }
        await addStockItemPanel.Locator("//select[@id='CategoryId']").ClickAsync();
        await _page.WaitForTimeoutAsync(500);
        await _page.Locator("#CategoryId").SelectOptionAsync(new SelectOptionValue
        {
            Label = category
        });
        // await _page.Locator($"//select[@id='CategoryId']//option[contains(.,'{category}')]").ClickAsync();
        await addStockItemPanel.Locator("//input[@id='Name']").FillAsync(name);

        // await _page.Locator("//input[@class='select2-input']").ClickAsync();
        if (unit.Contains("Invalid"))
        {
            await addStockItemPanel.Locator("//div[@id='s2id_ProductionUnitId']").ClickAsync();
            await _page.WaitForTimeoutAsync(500);
            // await _page.Locator("//input[@class='select2-input']").ClickAsync();
            await _page.Locator("//input[@class='select2-input select2-focused']").FillAsync(unit);
            return;

        }
        else if (unit != "")
        {
            await addStockItemPanel.Locator("//div[@id='s2id_ProductionUnitId']").ClickAsync();
            await _page.WaitForTimeoutAsync(500);
            // await _page.Locator("//input[@class='select2-input select2-focused']").FillAsync(unit);
            // await _page.WaitForTimeoutAsync(500);
            // await _page.Keyboard.PressAsync("Enter");
            await _page.Locator("//ul[@class='select2-results']").GetByText(unit, new () { Exact = true }).ClickAsync();


        }

        await addStockItemPanel.Locator("//input[@id='UnitCost']").FillAsync(cost);
        await addStockItemPanel.Locator("//input[@id='PartNumber']").FillAsync(partNum);
        await addStockItemPanel.Locator("//input[@id='QRCode']").FillAsync(QRcode);
        await addStockItemPanel.Locator("//textarea[@id='Description']").FillAsync(desc);

        await addStockItemPanel.Locator("//button[@id='btnSaveConsumable']").ClickAsync();





        await _page.WaitForTimeoutAsync(500);


    }
    public async Task categoryFilter(string category)
    {
        await _page.Locator("//select[@id='ddlConsumableCategory']").ClickAsync();
        await _page.WaitForTimeoutAsync(500);
        await _page.Locator("#ddlConsumableCategory").SelectOptionAsync(new SelectOptionValue
        {
            Label = category
        });
    }
    public async Task nameFilter(string name)
    {
        await _page.Locator("//input[@id='txtName']").FillAsync(name);
    }
    public async Task partNumFilter(string partNum)
    {
        await _page.Locator("//input[@id='Para_PartNumber']").FillAsync(partNum);
    }
    public async Task displayInactive(bool display)
    {
        if (display == true)
        {
            var check = await _page.Locator("//input[@id='Para_ShowInactive']").IsCheckedAsync();
            if (check == false)
            {
                await _page.Locator("//input[@id='Para_ShowInactive']").ClickAsync();
            }
        }
        else
        {
            var check = await _page.Locator("//input[@id='Para_ShowInactive']").IsCheckedAsync();
            if (check == true)
            {
                await _page.Locator("//input[@id='Para_ShowInactive']").ClickAsync();
            }
        }
    }
    public async Task combinedFilter(string category, string name, string partNum)
    {

        await categoryFilter(category);
        await nameFilter(name);
        await partNumFilter(partNum);
    }
    public async Task resetFilter()
    {
        await btnClear.ClickAsync();
        await _page.WaitForTimeoutAsync(5000);
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
    public async Task<List<string>> getCategoryList()
    {
        var categoryList = new List<string>();

        var rows = _page.Locator("//table[@id='tblConsumable']//tbody//tr");
        var count = await rows.CountAsync();

        for (int i = 1; i <= count; i++)
        {
            var category = await _page
                .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[2]")
                .InnerTextAsync();


            categoryList.Add(category);
        }

        return categoryList;

    }
    public async Task<List<string>> getNameList()
    {
        var nameList = new List<string>();

        var rows = _page.Locator("//table[@id='tblConsumable']//tbody//tr");
        var count = await rows.CountAsync();

        for (int i = 1; i <= count; i++)
        {
            var name = await _page
                .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[3]")
                .InnerTextAsync();


            nameList.Add(name);
        }

        return nameList;

    }
    public async Task<List<string>> getPartNumList()
    {
        var partNumList = new List<string>();

        var rows = _page.Locator("//table[@id='tblConsumable']//tbody//tr");
        var count = await rows.CountAsync();

        for (int i = 1; i <= count; i++)
        {
            var partNum = await _page
                .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[4]")
                .InnerTextAsync();


            partNumList.Add(partNum);
        }

        return partNumList;

    }
    public async Task<List<StockCatalogueTestData>> getListRecord()
    {
        var stockCatalogueList = new List<StockCatalogueTestData>();

        var rows = _page.Locator("//table[@id='tblConsumable']//tbody//tr");
        var count = await rows.CountAsync();

        for (int i = 1; i <= count; i++)
        {
            // string description;
            // try
            // {
            //     description = await _page
            //         .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[7]//a[@rel='tooltip']")
            //         .GetAttributeAsync("data-original-title") ?? "";

            // }
            // catch
            // {
            //     description="";
            // }
            var stockCatalogue = new StockCatalogueTestData
            {
                Category = await _page
                    .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[2]")
                    .InnerTextAsync(),
                Name = await _page
                    .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[3]")
                    .InnerTextAsync(),
                PartNum = await _page
                    .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[4]")
                    .InnerTextAsync(),
                Unit = await _page
                    .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[5]")
                    .InnerTextAsync(),
                Cost = await _page
                    .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[6]")
                    .InnerTextAsync(),
                Description = await _page
                    .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[7]//a[@rel='tooltip']").CountAsync() > 0 ?
                    await _page
                    .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[7]//a[@rel='tooltip']").GetAttributeAsync("data-original-title") ??
                    "" : "",


                SubType = await _page
                    .Locator($"//table[@id='tblConsumable']//tbody//tr[{i}]//td[8]")
                    .InnerTextAsync() ?? "",
            };

            stockCatalogueList.Add(stockCatalogue);
        }

        return stockCatalogueList;
    }
    public async Task Delete()
    {


        _page.Dialog += async (_, dialog) =>
 {
     await dialog.AcceptAsync();
 };
        await resultTable.Locator("//a[@data-original-title='Delete']").ClickAsync();
        await _page.WaitForTimeoutAsync(500);
    }

    public async Task editInformationOfStockItem(string feature, string newValue, bool initialStatus, bool changeStatus)
    {
        await _page.Locator("//a[@data-original-title='Edit']").ClickAsync();
        if (initialStatus != changeStatus)
        {
            await editStockItemPanel.Locator("//input[@id='IsActive']").ClickAsync();

        }
        switch (feature)
        {
            case "Category":
                await editStockItemPanel.Locator("//select[@id='CategoryId']").ClickAsync();
                await _page.WaitForTimeoutAsync(500);
                await _page.Locator("#CategoryId").SelectOptionAsync(new SelectOptionValue
                {
                    Label = newValue
                });
                // await _page.Locator($"//select[@id='CategoryId']//option[contains(.,'{newValue}')]").ClickAsync();
                break;
            case "Name":
                await editStockItemPanel.Locator("//input[@id='Name']").ClearAsync();
                await editStockItemPanel.Locator("//input[@id='Name']").FillAsync(newValue);
                break;
            case "Unit":
                await editStockItemPanel.Locator("//abbr[@class='select2-search-choice-close']").ClickAsync();
                if (newValue.Contains("Invalid"))
                {
                    await editStockItemPanel.Locator("//div[@id='s2id_ProductionUnitId']").ClickAsync();
                    await _page.WaitForTimeoutAsync(500);
                    // await _page.Locator("//input[@class='select2-input']").ClickAsync();
                    await _page.Locator("//input[@class='select2-input select2-focused']").FillAsync(newValue);
                    return;

                }
                else if (newValue != "")
                {
                    await editStockItemPanel.Locator("//div[@id='s2id_ProductionUnitId']").ClickAsync();
                    await _page.WaitForTimeoutAsync(500);
                    // await _page.Locator("//input[@class='select2-input']").ClickAsync();
                    // await _page.Locator("//input[@class='select2-input select2-focused']").FillAsync(newValue);
                    // await _page.WaitForTimeoutAsync(500);
                    // await _page.Keyboard.PressAsync("Enter");
                     await _page.Locator("//ul[@class='select2-results']").GetByText(newValue, new () { Exact = true }).ClickAsync();
                }

                break;
            case "Cost":
                await editStockItemPanel.Locator("//input[@id='UnitCost']").ClearAsync();
                await editStockItemPanel.Locator("//input[@id='UnitCost']").FillAsync(newValue);
                break;
            case "PartNum":
                await editStockItemPanel.Locator("//input[@id='PartNumber']").ClearAsync();
                await editStockItemPanel.Locator("//input[@id='PartNumber']").FillAsync(newValue);
                break;
            case "QRcode":
                await editStockItemPanel.Locator("//input[@id='QRCode']").ClearAsync();
                await editStockItemPanel.Locator("//input[@id='QRCode']").FillAsync(newValue);
                break;
            case "Description":
                await editStockItemPanel.Locator("//textarea[@id='Description']").ClearAsync();
                await editStockItemPanel.Locator("//textarea[@id='Description']").FillAsync(newValue);
                break;
            case "status":
                if (initialStatus != changeStatus)
                {
                    await editStockItemPanel.Locator("//input[@id='IsActive']").ClickAsync();

                }
                else
                {
                    return;
                }
                break;
        }
        await editStockItemPanel.Locator("//button[@id='btnSaveConsumable']").ClickAsync();
        await _page.WaitForTimeoutAsync(500);


    }


}
