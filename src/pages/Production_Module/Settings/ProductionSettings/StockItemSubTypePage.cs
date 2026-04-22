using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace PeoTest;

public class StockItemSubTypeTestData
{
    public string Name { get; set; }
    public string Cost { get; set; }
    public string Sequence { get; set; }
}

public class StockItemSubTypePage : CommonBasePage
{
    public StockItemSubTypePage(IPage page) : base(page) { }

    // =========================
    // 🔹 LOCATORS
    // =========================
    private ILocator addSubTypeBtn => _page.Locator("//a[@id='btnAddType']");
    private ILocator modalPanel => _page.Locator("//div[@id='divTypeDetails']");
    private ILocator typeInput => _page.Locator("//div[@id='divTypeDetails']//div[@id='s2id_ProductionTypeId']");
    private ILocator costInput => _page.Locator("//div[@id='divTypeDetails']//input[@id='UnitCost']");
    private ILocator sequenceInput => _page.Locator("//div[@id='divTypeDetails']//input[@id='Sequence']");
    private ILocator isActiveCheckbox => _page.Locator("//div[@id='divTypeDetails']//input[@id='IsActive']");
    private ILocator saveBtn => _page.Locator("//div[@id='divTypeDetails']//button[@id='btnSaveConsumableType']");
    private ILocator cancelBtn => _page.Locator("//div[@id='divTypeDetails']//button[@class='btn btn-default btn-flat md-close']");
    private ILocator messageSpan => _page.Locator("//div[@id='divTypeDetails']//span[@id='spnConsumableTypeMessage']");
    private ILocator resultTable => _page.Locator("//table[@id='tblEventSubType']");
    private ILocator processingStatus => _page.Locator("//div[@id='tblEventSubType_processing']");

    // =========================
    // 🔹 PAGE ACTIONS
    // =========================

    public async Task AddSubType(string typeName, string cost, string sequence, bool isActive = true)
    {
        await addSubTypeBtn.ClickAsync();
        await modalPanel.WaitForAsync();
        await _page.WaitForTimeoutAsync(500);

        // Select Type using Select2
        await SelectType(typeName);
        // Enter Cost
        await costInput.FillAsync(cost);

        // Enter Sequence
        await sequenceInput.FillAsync(sequence);

        // Handle IsActive checkbox
        if (isActive)
        {
            var isChecked = await isActiveCheckbox.IsCheckedAsync();
            if (!isChecked)
            {
                await isActiveCheckbox.ClickAsync();
            }
        }
        else
        {
            var isChecked = await isActiveCheckbox.IsCheckedAsync();
            if (isChecked)
            {
                await isActiveCheckbox.ClickAsync();
            }
        }

        // Save
        await saveBtn.ClickAsync();
        await _page.WaitForTimeoutAsync(1000);
    }
    public async Task<String> AddSubType( string cost, string sequence, bool isActive = true)
    {
        await addSubTypeBtn.ClickAsync();
        await modalPanel.WaitForAsync();
        await _page.WaitForTimeoutAsync(500);

        var typeName = await randomType();
        // Enter Cost
        await costInput.FillAsync(cost);

        // Enter Sequence
        await sequenceInput.FillAsync(sequence);

        // Handle IsActive checkbox
        if (isActive)
        {
            var isChecked = await isActiveCheckbox.IsCheckedAsync();
            if (!isChecked)
            {
                await isActiveCheckbox.ClickAsync();
            }
        }
        else
        {
            var isChecked = await isActiveCheckbox.IsCheckedAsync();
            if (isChecked)
            {
                await isActiveCheckbox.ClickAsync();
            }
        }

        // Save
        await saveBtn.ClickAsync();
        await _page.WaitForTimeoutAsync(500);
        return typeName;
    }

    public async Task EditSubType( string typeName, string cost, string sequence, bool isActive = true)
    {
        // Open the edit modal using JavaScript
        await _page.Locator("//a[@data-original-title='Edit']").First.ClickAsync();
        await modalPanel.WaitForAsync();
        await _page.WaitForTimeoutAsync(500);

        // Clear and enter new values
        await ClearType();
        await SelectType(typeName);
        await costInput.ClearAsync();
        await costInput.FillAsync(cost);
        await sequenceInput.ClearAsync();
        await sequenceInput.FillAsync(sequence);

        // Handle IsActive checkbox
        if (isActive)
        {
            var isChecked = await isActiveCheckbox.IsCheckedAsync();
            if (!isChecked)
            {
                await isActiveCheckbox.ClickAsync();
            }
        }
        else
        {
            var isChecked = await isActiveCheckbox.IsCheckedAsync();
            if (isChecked)
            {
                await isActiveCheckbox.ClickAsync();
            }
        }

        // Save
        await saveBtn.ClickAsync();
        await _page.WaitForTimeoutAsync(1000);
    }
    public async Task<String> EditSubType( string cost, string sequence, bool isActive = true)
    {
        // Open the edit modal using JavaScript
        await _page.Locator("//a[@data-original-title='Edit']").First.ClickAsync();
        await modalPanel.WaitForAsync();
        await _page.WaitForTimeoutAsync(500);

        // Clear and enter new values
        var typeName = await randomType();
        await costInput.ClearAsync();
        await costInput.FillAsync(cost);
        await sequenceInput.ClearAsync();
        await sequenceInput.FillAsync(sequence);

        // Handle IsActive checkbox
        if (isActive)
        {
            var isChecked = await isActiveCheckbox.IsCheckedAsync();
            if (!isChecked)
            {
                await isActiveCheckbox.ClickAsync();
            }
        }
        else
        {
            var isChecked = await isActiveCheckbox.IsCheckedAsync();
            if (isChecked)
            {
                await isActiveCheckbox.ClickAsync();
            }
        }

        // Save
        await saveBtn.ClickAsync();
        await _page.WaitForTimeoutAsync(1000);
        return typeName;
    }

    private async Task SelectType(string typeName )
    {
        await typeInput.ClickAsync();
        await _page.WaitForTimeoutAsync(500);
        await _page.Locator("//input[@class='select2-input select2-focused']").FillAsync(typeName);
        await _page.WaitForTimeoutAsync(500);
        await _page.Keyboard.PressAsync("Enter");
        
    }
    public async Task<String> randomType()
    {
        await typeInput.ClickAsync();
        await _page.WaitForTimeoutAsync(500);
        var random= RandomNumberGenerator.GetInt32(1, await _page.Locator("//ul[@class='select2-results']//div[@role='option']").CountAsync());
        await _page.Locator($"//ul[@class='select2-results']//div[@role='option']").Nth(random).ClickAsync();
        return await typeInput.InnerTextAsync();
        
    }

    public async Task ClearType()
    {
        // Remove existing selection by clicking the close button
        var closeBtn = _page.Locator("//div[@id='divTypeDetails']//abbr[@class='select2-search-choice-close']");
        var closeCount = await closeBtn.CountAsync();
        if (closeCount > 0)
        {
            await closeBtn.ClickAsync();
        }

       
    }

    public async Task Search(string keyword)
    {
        // Trigger search if needed
        await _page.Locator("//input[@id='txtName']").ClickAsync();
        await _page.Locator("//input[@id='txtName']").FillAsync(keyword);
        await _page.Locator("//button[@id='btnSearch']").ClickAsync();
        await processingStatus.WaitForAsync(new()
        {
            State = WaitForSelectorState.Hidden
        });
    }

    public async Task Cancel()
    {
        await cancelBtn.ClickAsync();
        await _page.WaitForTimeoutAsync(500);
    }

    public async Task<string> GetErrorMessage()
    {
        return await messageSpan.InnerTextAsync();
    }

    // =========================
    // 🔹 HELPER METHODS FOR VALIDATION TESTING
    // =========================

    public async Task OpenAddModal()
    {
        await addSubTypeBtn.ClickAsync();
        await modalPanel.WaitForAsync();
        await _page.WaitForTimeoutAsync(500);
    }
    public async Task OpenEditModal()
    {
        await _page.Locator("//a[@data-original-title='Edit']").First.ClickAsync();
        await modalPanel.WaitForAsync();
        await _page.WaitForTimeoutAsync(500);
    }

    public async Task EnterType(string typeName)
    {
        await SelectType(typeName);
    }

    public async Task EnterCost(string cost)
    {
        await costInput.FillAsync(cost);
    }

    public async Task EnterSequence(string sequence)
    {
        await sequenceInput.FillAsync(sequence);
    }
    public async Task ClearSequence()
    {
        await sequenceInput.ClearAsync();
    }

    public async Task ClickSave()
    {
        await saveBtn.ClickAsync();
        await _page.WaitForTimeoutAsync(1000);
    }

    public async Task ClearCost()
    {
        await costInput.ClearAsync();
    }

    

    public async Task<List<StockItemSubTypeTestData>> GetListRecords()
    {
        var subTypeList = new List<StockItemSubTypeTestData>();
        var rows = resultTable.Locator("//tbody//tr");
        var count = await rows.CountAsync();

        for (int i = 1; i <= count; i++)
        {
            var subType = new StockItemSubTypeTestData
            {
                Name = await _page
                    .Locator($"//table[@id='tblEventSubType']//tbody//tr[{i}]//td[2]")
                    .InnerTextAsync(),
                Cost = await _page
                    .Locator($"//table[@id='tblEventSubType']//tbody//tr[{i}]//td[3]")
                    .InnerTextAsync(),
                Sequence = await _page
                    .Locator($"//table[@id='tblEventSubType']//tbody//tr[{i}]//td[4]")
                    .InnerTextAsync()
            };

            subTypeList.Add(subType);
        }

        return subTypeList;
    }

    public async Task<List<string>> GetNameList()
    {
        var nameList = new List<string>();
        var rows = resultTable.Locator("//tbody//tr");
        var count = await rows.CountAsync();

        for (int i = 1; i <= count; i++)
        {
            var name = await _page
                .Locator($"//table[@id='tblEventSubType']//tbody//tr[{i}]//td[2]")
                .InnerTextAsync();

            nameList.Add(name);
        }

        return nameList;
    }

    public async Task<List<string>> GetCostList()
    {
        var costList = new List<string>();
        var rows = resultTable.Locator("//tbody//tr");
        var count = await rows.CountAsync();

        for (int i = 1; i <= count; i++)
        {
            var cost = await _page
                .Locator($"//table[@id='tblEventSubType']//tbody//tr[{i}]//td[3]")
                .InnerTextAsync();

            costList.Add(cost);
        }

        return costList;
    }

    public async Task<bool> VerifySubTypeExists(string typeName)
    {
        var nameList = await GetNameList();
        return nameList.Any(name => name.Equals(typeName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task Delete(int subTypeId)
    {
        _page.Dialog += async (_, dialog) =>
        {
            await dialog.AcceptAsync();
        };
        
        // Click delete button for specific row
        await _page.Locator($"//table[@id='tblEventSubType']//tbody//tr//a[@data-original-title='Delete']").ClickAsync();
        await _page.WaitForTimeoutAsync(500);
    }
    public async Task<string> GetFirstSubTypeName()
    {
        return await _page.Locator("//table[@id='tblEventSubType']//tbody//tr[1]//td[2]").InnerTextAsync();
    }
    public async Task<string> GetFirstSubTypeCost()
    {
        return await _page.Locator("//table[@id='tblEventSubType']//tbody//tr[1]//td[3]").InnerTextAsync();
    }
     public async Task<string> GetFirstSubTypeSequence()
    {
        return await _page.Locator("//table[@id='tblEventSubType']//tbody//tr[1]//td[4]").InnerTextAsync();
    }
}
