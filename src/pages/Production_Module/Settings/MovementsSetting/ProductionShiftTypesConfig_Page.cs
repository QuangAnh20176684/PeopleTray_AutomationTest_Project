using Microsoft.Playwright;
using Microsoft.VisualBasic;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PeoTest;

public class ProductionShiftTypesConfigTestData
{
    public string ShiftTypeName { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Hours { get; set; }



}
public class ProductionShiftTypesConfigPage : CommonBasePage
{
    public ProductionShiftTypesConfigPage(IPage page) : base(page) { }
    // =========================
    // 🔹 LOCATORs
    // =========================
    private ILocator addProductionShiftTypePanel => _page.Locator("//div[@class='DTE DTE_Action_Create' and contains(.,'Add Production Shift Type')]");
    private ILocator addShiftTypesBtn => _page.Locator("//a[@id='ToolTables_tblProductionShiftTypes_0' and contains(.,'New')]");
     private ILocator deleteShiftTypesBtn => _page.Locator("//a[@id='ToolTables_tblProductionShiftTypes_1' and contains(.,'Delete')]"); 
    private ILocator prodcutionShiftTypesTable => _page.Locator("//table[@id='tblProductionShiftTypes']");




    // =========================
    // 🔹 ACTIONS
    // =========================
    public async Task SwitchToOptions(string optionName, string subOptionName)
    {
        await _page.Locator($"//div[@class='content' and contains(.,'Settings')]//label[contains(.,'{optionName}')]").ClickAsync();
        await _page.WaitForTimeoutAsync(1000);
        await _page.Locator($"//div[@class='content' and contains(.,'Settings')]//label[contains(.,'{optionName}')]/following-sibling::ul//li[contains(.,'{subOptionName}')]").ClickAsync();

    }// Switch to sub options in left menu bar in each module, for example: Settings -> Production Settings, then subOptionName
    public async Task addProductionShiftType(string shiftTypeName)
    {
        await addShiftTypesBtn.ClickAsync();
        await _page.WaitForTimeoutAsync(500);
        await addProductionShiftTypePanel.Locator("#DTE_Field_ShiftTypeId").ClickAsync();
        await _page.WaitForTimeoutAsync(500);
        await addProductionShiftTypePanel.Locator("#DTE_Field_ShiftTypeId").SelectOptionAsync(new SelectOptionValue
        {
            Label = shiftTypeName
        });
        
        await addProductionShiftTypePanel.ClickAsync();
        await addProductionShiftTypePanel.Locator("//button[@class='btn' and contains(.,'Create')]").ClickAsync();
        await _page.WaitForTimeoutAsync(500);
    }
    public async Task<List<ProductionShiftTypesConfigTestData>> getListRecord()
    {
        List<ProductionShiftTypesConfigTestData> list = new List<ProductionShiftTypesConfigTestData>();
        var rowCount = await prodcutionShiftTypesTable.Locator("tbody tr").CountAsync();
        for (int i = 1; i <= rowCount; i++)
        {
            var ProductionShiftTypesConfig = new ProductionShiftTypesConfigTestData
            {
                ShiftTypeName = await _page
                    .Locator($"//table[@id='tblProductionShiftTypes']//tbody//tr[{i}]//td[2]")
                    .InnerTextAsync(),
                StartTime = await _page
                    .Locator($"//table[@id='tblProductionShiftTypes']//tbody//tr[{i}]//td[3]")
                    .InnerTextAsync(),
                EndTime = await _page
                    .Locator($"//table[@id='tblProductionShiftTypes']//tbody//tr[{i}]//td[4]")
                    .InnerTextAsync(),
                Hours = await _page
                    .Locator($"//table[@id='tblProductionShiftTypes']//tbody//tr[{i}]//td[5]")
                    .InnerTextAsync()
            };
            list.Add(ProductionShiftTypesConfig);
        }
        return list;
    }
    public async Task deleteShiftType(string shiftTypeName)
    {
        await selectShift(shiftTypeName);
        await deleteShiftTypesBtn.ClickAsync();
        await _page.Locator("//button[contains(.,'Delete')]").ClickAsync();

    }
    public async Task selectShift(string shiftTypeName)
    {
        await prodcutionShiftTypesTable.Locator($"//tbody//tr[contains(.,'{shiftTypeName}')]//td[1]").First.ClickAsync();
    }
    public async Task searchForShiftType(string shiftTypeName)
    {
        await _page.Locator("//input[@type='search']").FillAsync(shiftTypeName);
        await _page.WaitForTimeoutAsync(500);

    }
}