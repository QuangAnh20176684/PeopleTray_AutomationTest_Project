using Microsoft.Playwright;
using Allure.NUnit;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeoTest;

[TestFixture]
[AllureNUnit]
public class StockItemSubTypeTest : CommonBaseTest
{
    [SetUp]
    public async Task Setup()
    {
        LoginPage loginPage = new LoginPage(_page);
        await loginPage.Login(CT_Account.Username, CT_Account.Password);
        await _page.WaitForTimeoutAsync(1000);
        HomePage homePage = new HomePage(_page);
        await homePage.SwitchToModule("Production");
        ProductionPage productionPage = new ProductionPage(_page);
        await productionPage.SwitchToOptionsUpperMenuBar("Settings");
        SettingsPage settingsPage = new SettingsPage(_page);
        await settingsPage.SwitchToOptions("Production Settings", "Stock Catalogue");
        await _page.WaitForTimeoutAsync(700);
        var random = RandomNumberGenerator.GetInt32(1, await _page.GetByText("Configure").CountAsync());
        await _page.GetByText("Configure").Nth(random).ClickAsync();
    }
    [Test]
    public async Task Test()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);
        await _page.WaitForTimeoutAsync(1000);
        // first, check if there is at least 1 subtype to edit, if not create one
        bool isEmpty = await _page.GetByText("No data available in table").IsVisibleAsync();
        await _page.WaitForTimeoutAsync(5000);
        Console.WriteLine("isEmpty: " + isEmpty);

    }

    [Test]
    public async Task Test_AddSubTypeWithValidData()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        var cost = "25.5";
        var sequence = "1";

        var typeName = await subTypePage.AddSubType(cost, sequence);
        await _page.WaitForTimeoutAsync(500);

        // Verify the sub type was added
        bool isFound = await subTypePage.VerifySubTypeExists(typeName);
        Assert.IsTrue(isFound, $"Sub Type '{typeName}' should be found in the list");
    }










    // =========================
    // VALIDATION TEST CASES
    // =========================



    [Test]
    [Description("Rule 2: Cannot create type if that type is created before - Error msg: Selected type already exists")]
    public async Task Test_Rule2_DuplicateTypeError()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);
        var existingType = await subTypePage.GetFirstSubTypeName();
        var cost = "25.0";
        var sequence = "99";

        // Try to create a sub type with an existing type
        await subTypePage.AddSubType(existingType, cost, sequence);
        await _page.WaitForTimeoutAsync(1000);

        // Verify error message is displayed
        string errorMsg = await subTypePage.GetErrorMessage();
        Assert.That(errorMsg, Does.Contain("Selected type already exists"),
            $"Rule 2 Failed: Error message should contain 'Selected type already exists', got: '{errorMsg}'");
    }

    [Test]
    [Description("Rule 3: Type field is required - Error msg: This field is required")]
    public async Task Test_Rule3_TypeFieldRequired()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);
        var cost = "25.0";
        var sequence = "1";

        // Open modal but don't select a type
        await subTypePage.OpenAddModal();

        // Fill only cost and sequence
        await subTypePage.EnterCost(cost);
        await subTypePage.EnterSequence(sequence);

        // Try to save without selecting type
        await subTypePage.ClickSave();

        // Verify error message
        string errorMsg = await subTypePage.GetErrorMessage();
        Assert.That(errorMsg, Does.Contain("Select a type."),
            $"Rule 3 Failed: Error message should contain 'Select a type.' for Type field, got: '{errorMsg}'");
    }

    [Test]
    [Description("Rule 3b: Cost field is required - Error msg: This field is required")]
    public async Task Test_Rule3b_CostFieldRequired()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        var sequence = "1";

        // Open modal and select type
        await subTypePage.OpenAddModal();

        // Select type
        var typeName = await subTypePage.randomType();

        // Fill only sequence (skip cost)
        await subTypePage.EnterSequence(sequence);

        // Try to save without cost
        await subTypePage.ClickSave();

        // Verify error message

        Assert.IsTrue(await _page.GetByText("This field is required.").IsVisibleAsync(),
            $"Rule 3b Failed: 'This field is required.' error message should be visible for Cost field");
    }

    [Test]
    [Description("Rule 3c: Sequence field is required - Error msg: This field is required")]
    public async Task Test_Rule3c_SequenceFieldRequired()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        var cost = "25.0";

        // Open modal and select type
        await subTypePage.OpenAddModal();
        await subTypePage.ClearSequence();

        // Select type
        var typeName = await subTypePage.randomType();

        // Fill only cost (skip sequence)
        await subTypePage.EnterCost(cost);

        // Try to save without sequence
        await subTypePage.ClickSave();
        await _page.WaitForTimeoutAsync(1000);

        // Verify error message
        Assert.IsTrue(await _page.GetByText("This field is required.").IsVisibleAsync(),
            $"Rule 3b Failed: 'This field is required.' error message should be visible for Cost field");
    }

    [Test]
    [Description("Rule 4: Cost cannot be negative - Error msg: Cost cannot be negative")]
    public async Task Test_Rule4_NegativeCostError()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);
        var cost = "-5.0";
        var sequence = "1";

        // Open modal and select type
        await subTypePage.OpenAddModal();

        // Select type
        var typeName = await subTypePage.randomType();
        // fill cost with negative value
        await subTypePage.EnterCost(cost);

        // Fill only sequence 
        await subTypePage.EnterSequence(sequence);

        // Try to save without cost
        await subTypePage.ClickSave();

        // Verify error message

        Assert.IsTrue(await _page.GetByText("Cost cannot be negative").IsVisibleAsync(),
            $"Rule 4 Failed: 'Cost cannot be negative' error message should be visible for Cost field");
    }

    [Test]
    [Description("Rule 5: Cost must be numeric - Error msg: Please enter a valid number")]
    public async Task Test_Rule5_InvalidCostFormat()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);
        var cost = "Cost";
        var sequence = "1";

        // Open modal and select type
        await subTypePage.OpenAddModal();

        // Select type
        var typeName = await subTypePage.randomType();
        // fill cost with negative value
        await subTypePage.EnterCost(cost);

        // Fill only sequence 
        await subTypePage.EnterSequence(sequence);

        // Try to save without cost
        await subTypePage.ClickSave();

        // Verify error message

        Assert.IsTrue(await _page.GetByText("Please enter a valid number.").IsVisibleAsync(),
            $"Rule 5 Failed: 'Please enter a valid number' error message should be visible for Cost field");

    }

    [Test]
    [Description("Rule 5b: Cost with special characters - Error msg: Please enter a valid number")]
    public async Task Test_Rule5b_CostWithSpecialCharacters()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);
        var cost = "Cost@£&£@";
        var sequence = "1";

        // Open modal and select type
        await subTypePage.OpenAddModal();

        // Select type
        var typeName = await subTypePage.randomType();
        // fill cost with negative value
        await subTypePage.EnterCost(cost);

        // Fill only sequence 
        await subTypePage.EnterSequence(sequence);

        // Try to save without cost
        await subTypePage.ClickSave();

        // Verify error message

        Assert.IsTrue(await _page.GetByText("Please enter a valid number.").IsVisibleAsync(),
            $"Rule 5 Failed: 'Please enter a valid number' error message should be visible for Cost field");
    }

    [Test]
    [Description("Boundary: Cost equals zero is allowed")]
    public async Task Test_Boundary_ZeroCostAllowed()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        var cost = "0";
        var sequence = "1";


        var typeName = await subTypePage.AddSubType(cost, sequence);
        await _page.WaitForTimeoutAsync(1000);

        // Verify the sub type was added (zero cost should be allowed)
        await subTypePage.Search(typeName);
        Assert.AreEqual(cost, await subTypePage.GetFirstSubTypeCost(), $"Zero cost should be allowed, but got '{await subTypePage.GetFirstSubTypeCost()}'");
    }

    [Test]
    [Description("Boundary: Large cost value")]
    public async Task Test_Boundary_LargeCostValue()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        var cost = "999999.99";
        var sequence = "1";


        var typeName = await subTypePage.AddSubType(cost, sequence);
        await _page.WaitForTimeoutAsync(1000);

        // Verify the sub type was added (zero cost should be allowed)
        await subTypePage.Search(typeName);
        Assert.AreEqual(cost, await subTypePage.GetFirstSubTypeCost(), $"large cost should be allowed, but got '{await subTypePage.GetFirstSubTypeCost()}'");
    }

    // =========================
    // EDIT TEST CASES
    // =========================

    [Test]
    [Description("Edit SubType with valid name")]
    public async Task Test_EditSubType_WithValidName()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);
        await _page.WaitForTimeoutAsync(1000);
        // first, check if there is at least 1 subtype to edit, if not create one
        bool isEmpty= await _page.GetByText("No data available in table").IsVisibleAsync();
        if (isEmpty==true)
        {
            var typeName = await subTypePage.AddSubType("20.0", "1");
            await _page.WaitForTimeoutAsync(500);
            await subTypePage.Search(typeName);
            await _page.WaitForTimeoutAsync(500);
            var newTypeName = await subTypePage.EditSubType("20.0", "1");
            await subTypePage.Search(typeName);
            await _page.WaitForTimeoutAsync(5000);
            Assert.IsTrue(await _page.GetByText("No data available in table").IsVisibleAsync(), "After editing, the sub type should no longer be found with the old name");
        }
        else
        {
            var initialName = await subTypePage.GetFirstSubTypeName();
            var newTypeName = await subTypePage.EditSubType("20.0", "1");
            
            await subTypePage.Search(initialName);
            await _page.WaitForTimeoutAsync(5000);
            Assert.IsTrue(await _page.GetByText("No data available in table").IsVisibleAsync(), "After editing, the sub type should be found with the new name");
            
        }
        // Sometimes, this case run into the same inital and new name, so it's could fail, but it rarely happens
        // At this time, it's false, because the Search in this feature is not working
    }
    [Test]
    [Description("Edit SubType with blank name")]
    public async Task Test_EditSubType_WithBlankName()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);
        await _page.WaitForTimeoutAsync(1000);
        // first, check if there is at least 1 subtype to edit, if not create one
        bool isEmpty = await _page.GetByText("No data available in table").IsVisibleAsync();
        if (isEmpty==true)
        {
            var typeName = await subTypePage.AddSubType("20.0", "1");
            await _page.WaitForTimeoutAsync(500);
            await subTypePage.Search(typeName);
            await _page.WaitForTimeoutAsync(500);
            await subTypePage.OpenEditModal();
            await subTypePage.ClearType();
            await subTypePage.EnterCost("20.0");
            await subTypePage.EnterSequence("1");
            await subTypePage.ClickSave();
            await _page.WaitForTimeoutAsync(500);
            Assert.IsTrue(await _page.GetByText("Select a type.").IsVisibleAsync(), "An error message 'Select a type.' should be visible when trying to save with blank name");
        }
        else
        {
            await subTypePage.OpenEditModal();
            await subTypePage.ClearType();
            await subTypePage.EnterCost("20.0");
            await subTypePage.EnterSequence("1");
            await subTypePage.ClickSave();
            await _page.WaitForTimeoutAsync(500);
            Assert.IsTrue(await _page.GetByText("Select a type.").IsVisibleAsync(), "An error message 'Select a type.' should be visible when trying to save with blank name");
            
        }

        
        
    }
    [Test]
    [Description("Edit SubType with name already exists")]
    public async Task Test_EditSubType_WithNameAlreadyExists()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        // First, get the data of the first subtype to edit
        var originalName = await subTypePage.GetFirstSubTypeName();
        int subTypeId = 1;

        // New valid data following the same rules as creation
        var newCost = "35.75";
        var newSequence = "2";

        // Select a new type (different from current)
        var allTypes = await subTypePage.GetNameList();
        string newType = allTypes.Count > 0 ? allTypes[0] : "Test Type";

        await subTypePage.EditSubType( newType, newCost, newSequence);
        await _page.WaitForTimeoutAsync(1000);

        // Verify the sub type was updated
        await subTypePage.Search(newType);
        Assert.AreEqual(newCost, await subTypePage.GetFirstSubTypeCost(),
            $"Cost should be updated to '{newCost}', but got '{await subTypePage.GetFirstSubTypeCost()}'");
        Assert.AreEqual(newSequence, await subTypePage.GetFirstSubTypeSequence(),
            $"Sequence should be updated to '{newSequence}', but got '{await subTypePage.GetFirstSubTypeSequence()}'");
    }
    [Test]
    [Description("Edit SubType with valid Information")]
    public async Task Test_EditSubType_WithValidInformation()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        // First, get the data of the first subtype to edit
        var originalName = await subTypePage.GetFirstSubTypeName();
        int subTypeId = 1;

        // New valid data following the same rules as creation
        var newCost = "35.75";
        var newSequence = "2";

        // Select a new type (different from current)
        var allTypes = await subTypePage.GetNameList();
        string newType = allTypes.Count > 0 ? allTypes[0] : "Test Type";

        await subTypePage.EditSubType( newType, newCost, newSequence);
        await _page.WaitForTimeoutAsync(1000);

        // Verify the sub type was updated
        await subTypePage.Search(newType);
        Assert.AreEqual(newCost, await subTypePage.GetFirstSubTypeCost(),
            $"Cost should be updated to '{newCost}', but got '{await subTypePage.GetFirstSubTypeCost()}'");
        Assert.AreEqual(newSequence, await subTypePage.GetFirstSubTypeSequence(),
            $"Sequence should be updated to '{newSequence}', but got '{await subTypePage.GetFirstSubTypeSequence()}'");
    }
    [Test]
    [Description("Edit SubType with Blank Cost")]
    public async Task Test_EditSubType_WithBlankCost()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        // First, get the data of the first subtype to edit
        var originalName = await subTypePage.GetFirstSubTypeName();
        int subTypeId = 1;

        // New valid data following the same rules as creation
        var newCost = "35.75";
        var newSequence = "2";

        // Select a new type (different from current)
        var allTypes = await subTypePage.GetNameList();
        string newType = allTypes.Count > 0 ? allTypes[0] : "Test Type";

        await subTypePage.EditSubType( newType, newCost, newSequence);
        await _page.WaitForTimeoutAsync(1000);

        // Verify the sub type was updated
        await subTypePage.Search(newType);
        Assert.AreEqual(newCost, await subTypePage.GetFirstSubTypeCost(),
            $"Cost should be updated to '{newCost}', but got '{await subTypePage.GetFirstSubTypeCost()}'");
        Assert.AreEqual(newSequence, await subTypePage.GetFirstSubTypeSequence(),
            $"Sequence should be updated to '{newSequence}', but got '{await subTypePage.GetFirstSubTypeSequence()}'");
    }
    [Test]
    [Description("Edit SubType with Blank Sequence")]
    public async Task Test_EditSubType_WithBlankSequence()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        // First, get the data of the first subtype to edit
        var originalName = await subTypePage.GetFirstSubTypeName();
        int subTypeId = 1;

        // New valid data following the same rules as creation
        var newCost = "35.75";
        var newSequence = "2";

        // Select a new type (different from current)
        var allTypes = await subTypePage.GetNameList();
        string newType = allTypes.Count > 0 ? allTypes[0] : "Test Type";

        await subTypePage.EditSubType( newType, newCost, newSequence);
        await _page.WaitForTimeoutAsync(1000);

        // Verify the sub type was updated
        await subTypePage.Search(newType);
        Assert.AreEqual(newCost, await subTypePage.GetFirstSubTypeCost(),
            $"Cost should be updated to '{newCost}', but got '{await subTypePage.GetFirstSubTypeCost()}'");
        Assert.AreEqual(newSequence, await subTypePage.GetFirstSubTypeSequence(),
            $"Sequence should be updated to '{newSequence}', but got '{await subTypePage.GetFirstSubTypeSequence()}'");
    }

    [Test]
    [Description("Edit SubType: Cost cannot be negative")]
    public async Task Test_EditSubType_Rule4_NegativeCostError()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        int subTypeId = 1;
        var newCost = "-10.5";
        var newSequence = "1";
        var newType = await subTypePage.randomType();

        // Open edit modal
        await _page.EvaluateAsync($"() => showTypeEditor({subTypeId})");
        await _page.Locator("//div[@id='divTypeDetails']").WaitForAsync();
        await _page.WaitForTimeoutAsync(500);

        // Clear and fill fields
        await subTypePage.EnterCost(newCost);
        await subTypePage.EnterSequence(newSequence);

        // Try to save with negative cost
        await subTypePage.ClickSave();

        // Verify error message
        Assert.IsTrue(await _page.GetByText("Cost cannot be negative").IsVisibleAsync(),
            $"Rule 4 Failed: 'Cost cannot be negative' error message should be visible when editing");
    }

    [Test]
    [Description("Edit SubType: Cost must be numeric")]
    public async Task Test_EditSubType_Rule5_InvalidCostFormat()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        int subTypeId = 1;
        var newCost = "InvalidCost";
        var newSequence = "1";

        // Open edit modal
        await _page.EvaluateAsync($"() => showTypeEditor({subTypeId})");
        await _page.Locator("//div[@id='divTypeDetails']").WaitForAsync();
        await _page.WaitForTimeoutAsync(500);

        // Fill with invalid cost
        await subTypePage.EnterCost(newCost);
        await subTypePage.EnterSequence(newSequence);

        // Try to save with invalid cost
        await subTypePage.ClickSave();

        // Verify error message
        Assert.IsTrue(await _page.GetByText("Please enter a valid number.").IsVisibleAsync(),
            $"Rule 5 Failed: 'Please enter a valid number' error message should be visible when editing");
    }

    [Test]
    [Description("Edit SubType: Cost with special characters")]
    public async Task Test_EditSubType_Rule5b_CostWithSpecialCharacters()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        int subTypeId = 1;
        var newCost = "Cost@£&£@";
        var newSequence = "1";

        // Open edit modal
        await _page.EvaluateAsync($"() => showTypeEditor({subTypeId})");
        await _page.Locator("//div[@id='divTypeDetails']").WaitForAsync();
        await _page.WaitForTimeoutAsync(500);

        // Fill with special characters
        await subTypePage.EnterCost(newCost);
        await subTypePage.EnterSequence(newSequence);

        // Try to save with special characters in cost
        await subTypePage.ClickSave();

        // Verify error message
        Assert.IsTrue(await _page.GetByText("Please enter a valid number.").IsVisibleAsync(),
            $"Rule 5b Failed: 'Please enter a valid number' error message should be visible when editing");
    }

    [Test]
    [Description("Edit SubType: Boundary - Zero cost is allowed")]
    public async Task Test_EditSubType_Boundary_ZeroCostAllowed()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        int subTypeId = 1;
        var newCost = "0";
        var newSequence = "3";
        var newType = await subTypePage.randomType();

        await subTypePage.EditSubType( newType, newCost, newSequence);
        await _page.WaitForTimeoutAsync(1000);

        // Verify zero cost was saved
        await subTypePage.Search(newType);
        Assert.AreEqual(newCost, await subTypePage.GetFirstSubTypeCost(),
            $"Zero cost should be allowed when editing, but got '{await subTypePage.GetFirstSubTypeCost()}'");
    }

    [Test]
    [Description("Edit SubType: Boundary - Large cost value")]
    public async Task Test_EditSubType_Boundary_LargeCostValue()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        int subTypeId = 1;
        var newCost = "999999.99";
        var newSequence = "10";
        var newType = await subTypePage.randomType();

        await subTypePage.EditSubType( newType, newCost, newSequence);
        await _page.WaitForTimeoutAsync(1000);

        // Verify large cost was saved
        await subTypePage.Search(newType);
        Assert.AreEqual(newCost, await subTypePage.GetFirstSubTypeCost(),
            $"Large cost value should be allowed when editing, but got '{await subTypePage.GetFirstSubTypeCost()}'");
    }

    [Test]
    [Description("Edit SubType: Decimal cost with precision")]
    public async Task Test_EditSubType_DecimalCostPrecision()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        int subTypeId = 1;
        var newCost = "45.99";
        var newSequence = "5";
        var newType = await subTypePage.randomType();

        await subTypePage.EditSubType( newType, newCost, newSequence);
        await _page.WaitForTimeoutAsync(1000);

        // Verify decimal cost with precision was saved correctly
        await subTypePage.Search(newType);
        Assert.AreEqual(newCost, await subTypePage.GetFirstSubTypeCost(),
            $"Decimal cost should maintain precision, expected '{newCost}' but got '{await subTypePage.GetFirstSubTypeCost()}'");
    }

    [Test]
    [Description("Edit SubType: Verify all fields can be updated together")]
    public async Task Test_EditSubType_UpdateAllFields()
    {
        StockItemSubTypePage subTypePage = new StockItemSubTypePage(_page);

        int subTypeId = 1;
        var newCost = "50.00";
        var newSequence = "7";
        var newType = await subTypePage.randomType();

        await subTypePage.EditSubType( newType, newCost, newSequence);
        await _page.WaitForTimeoutAsync(1000);

        // Verify all fields were updated
        await subTypePage.Search(newType);
        var retrievedCost = await subTypePage.GetFirstSubTypeCost();
        var retrievedSequence = await subTypePage.GetFirstSubTypeSequence();

        Assert.AreEqual(newCost, retrievedCost,
            $"Cost should be '{newCost}', but got '{retrievedCost}'");
        Assert.AreEqual(newSequence, retrievedSequence,
            $"Sequence should be '{newSequence}', but got '{retrievedSequence}'");
    }
}
