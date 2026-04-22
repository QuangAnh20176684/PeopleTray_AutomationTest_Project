using Microsoft.Playwright;
using Microsoft.VisualBasic;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PeoTest;

public class ProductionShiftGroupTestData
{

    public string Name { get; set; }



    public string Description { get; set; }
    public string ShiftTypes { get; set; }

}
public class ProductionShiftGroupPage : CommonBasePage
{
    public ProductionShiftGroupPage(IPage page) : base(page) { }

    // =========================
    // 🔹 LOCATORs
    // =========================
    private ILocator addProductionPanel => _page.Locator("//div[@class='DTE DTE_Action_Create' and contains(.,'Add Production')]");
    private ILocator addProductionBtn => _page.Locator("//a[@id='ToolTables_tblProductionShiftGroups_0' and contains(.,'New')]");
    private ILocator prodcutionShiftGroupTable => _page.Locator("//table[@id='tblProductionShiftGroups']");
    private ILocator editBtn => _page.Locator("//a[@id='ToolTables_tblProductionShiftGroups_1' and contains(.,'Edit')]");
    private ILocator editProductionPanel => _page.Locator("//div[@class='DTE DTE_Action_Edit' and contains(.,'Edit Equipment Compliance')]");
    

    // =========================
    // 🔹 ACTIONS
    // =========================


    public async Task SwitchToOptions(string optionName, string subOptionName)
    {
        await _page.Locator($"//div[@class='content' and contains(.,'Settings')]//label[contains(.,'{optionName}')]").ClickAsync();
        await _page.WaitForTimeoutAsync(1000);
        await _page.Locator($"//div[@class='content' and contains(.,'Settings')]//label[contains(.,'{optionName}')]/following-sibling::ul//li[contains(.,'{subOptionName}')]").ClickAsync();

    }// Switch to sub options in left menu bar in each module, for example: Settings -> Production Settings, then subOptionName
    public async Task addProductionShiftGroup(string name, string desc, bool isInactive)
    {
        await addProductionBtn.ClickAsync();
        await addProductionPanel.Locator("//input[@id='DTE_Field_Name']").ClickAsync();
        await addProductionPanel.Locator("//input[@id='DTE_Field_Name']").FillAsync(name);
        await addProductionPanel.Locator("//textarea[@id='DTE_Field_Description']").ClickAsync();
        await addProductionPanel.Locator("//textarea[@id='DTE_Field_Description']").FillAsync(desc);

        if (isInactive == false)
        {
            await addProductionPanel.Locator("//input[@id='DTE_Field_IsActive_0']").ClickAsync();
        }
        await addProductionPanel.Locator("//button[@class='btn' and contains(.,'Create')]").ClickAsync();
    }
    public async Task<List<ProductionShiftGroupTestData>> getListRecord()
    {
        List<ProductionShiftGroupTestData> list = new List<ProductionShiftGroupTestData>();
        var rowCount = await prodcutionShiftGroupTable.Locator("//tbody//tr").CountAsync();
        for (int i = 1; i <= rowCount; i++)
        {
            var ProductionShiftGroup = new ProductionShiftGroupTestData
            {
                Name = await _page
                    .Locator($"//table[@id='tblProductionShiftGroups']//tbody//tr[{i}]//td[2]")
                    .InnerTextAsync(),
                Description = await _page
                    .Locator($"//table[@id='tblProductionShiftGroups']//tbody//tr[{i}]//td[3]")
                    .InnerTextAsync(),
                ShiftTypes = await _page
                    .Locator($"//table[@id='tblProductionShiftGroups']//tbody//tr[{i}]//td[4]")
                    .InnerTextAsync()
            };
            list.Add(ProductionShiftGroup);
        }
        return list;
    }

    public async Task searchFor(string value)
    {
        await _page.Locator("//label[contains(.,'Search:')]//input").ClickAsync();
        await _page.Locator("//label[contains(.,'Search:')]//input").FillAsync(value);
        await _page.WaitForTimeoutAsync(500);
        // No need to click search button because the search will be triggered automatically after filling 


    }

    public async Task editProductionShiftGroup(string existingName, string newName, string newDesc)
    {
        await searchFor(existingName);
        await _page.Locator("//table[@id='tblShiftGroup']//tbody//tr[1]//td[4]//a[contains(@class,'edit')]").ClickAsync();
        await _page.Locator("//input[@id='Name']").FillAsync(newName);
        await _page.Locator("//textarea[@id='Description']").FillAsync(newDesc);
        await _page.Locator("//button[@id='btnSave']").ClickAsync();
    }
    public async Task deleteProductionShiftGroup(string name)
    {
        await searchFor(name);
        await _page.Locator("//table[@id='tblShiftGroup']//tbody//tr[1]//td[4]//a[contains(@class,'delete')]").ClickAsync();
        await _page.Locator("//button[@id='btnConfirmDelete']").ClickAsync();
    }
    public async Task showInactive()
    {
       await _page.Locator("#chkShowInactive").ClickAsync();
       await _page.WaitForTimeoutAsync(500);

    }
    public async Task configureShiftTypes(string groupName)
    //this func configure the record has names has been passed in
    {
        
        await _page.Locator($"//table[@id='tblProductionShiftGroups']//tbody//tr[contains(.,'{groupName}')]").GetByText("Configure").ClickAsync();
        // after click configure button, it will navigate to another page, so no need to wait for the pop up, just wait for the new page to load
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

    }
    public async Task configureShiftTypes()
    // this func configure the first record 
    {
        await _page.Locator("//table[@id='tblProductionShiftGroups']//tbody//tr[1]").GetByText("Configure").ClickAsync();
        // after click configure button, it will navigate to another page, so no need to wait for the pop up, just wait for the new page to load
        await _page.WaitForTimeoutAsync(500);
    }
    
    public async Task<bool> getInitialStatus()
    {
        await _page.WaitForTimeoutAsync(1000);
        return await editProductionPanel.Locator("#DTE_Field_IsActive_0").IsCheckedAsync();
        
    }
    public async Task clickEditBtn()
    {
        await editBtn.ClickAsync();
        await _page.WaitForTimeoutAsync(500);
    }
    public async Task editProductionShiftGroup(string newName, string newDesc, bool isInactive)
    {
        await _page.Locator("//input[@id='DTE_Field_Name']").ClickAsync();
        await _page.Locator("//input[@id='DTE_Field_Name']").ClearAsync();
        await _page.Locator("//input[@id='DTE_Field_Name']").FillAsync(newName);

        await _page.Locator("//textarea[@id='DTE_Field_Description']").ClickAsync();
        await _page.Locator("//textarea[@id='DTE_Field_Description']").ClearAsync();
        await _page.Locator("//textarea[@id='DTE_Field_Description']").FillAsync(newDesc);
        

        if (isInactive == false && await getInitialStatus()==true)
        {
            await editProductionPanel.Locator("//input[@id='DTE_Field_IsActive_0']").ClickAsync();
        }
        else if (isInactive == true && await getInitialStatus() == false)
        {
            await editProductionPanel.Locator("//input[@id='DTE_Field_IsActive_0']").ClickAsync();
        }
        await editProductionPanel.GetByText("Update").ClickAsync();

        
    }
    public async Task selectFirstProductionShiftGroup()
    {
        await prodcutionShiftGroupTable.Locator("tbody tr td").First.ClickAsync();
        
    }
    
}


