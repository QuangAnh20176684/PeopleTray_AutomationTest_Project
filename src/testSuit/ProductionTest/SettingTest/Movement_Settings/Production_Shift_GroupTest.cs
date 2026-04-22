using Microsoft.Playwright;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeoTest;

[TestFixture]

public class Production_Shift_GroupTest : CommonBaseTest
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
        await settingsPage.SwitchToOptions("Movements Settings", "Production Shift Group");
        await _page.WaitForTimeoutAsync(2000);


    }
    [Test]
    public async Task Test()
    {
        ProductionShiftGroupPage productionPage = new ProductionShiftGroupPage(_page);
        // List<ProductionShiftGroupTestData> list = await productionPage.getListRecord();
        // foreach (var item in list)
        // {
        //     Console.WriteLine($"Name: {item.Name} - Description: {item.Description}");
        // }
        await productionPage.selectFirstProductionShiftGroup();
        await _page.WaitForTimeoutAsync(5000);


    }
    [Test]
    public async Task AddNewProductionShiftGroupWithInactiveStatus()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        var shiftName = generateHelper.GenerateRandomString(5, "ShiftName_");
        var shiftDesc = generateHelper.GenerateRandomString(10, "Description_");
        await productionShiftGroupPage.addProductionShiftGroup(shiftName, shiftDesc, false);
        await _page.WaitForTimeoutAsync(500);
        await productionShiftGroupPage.searchFor(shiftName);
        await productionShiftGroupPage.showInactive();
        
         Assert.IsTrue(await validateHelper.IsValuePresent(await productionShiftGroupPage.getListRecord(), "Name", shiftName));
        // Only name is unique, so we just check name to verify the record is created successfully, Description and ShiftTypes are not necessary to check because they are not unique and can be same with other record

    }
    [Test]
    public async Task AddNewProductionShiftGroupWithActiveStatus()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        var shiftName = generateHelper.GenerateRandomString(5, "ShiftName_");
        var shiftDesc = generateHelper.GenerateRandomString(10, "Description_");
        await productionShiftGroupPage.addProductionShiftGroup(shiftName, shiftDesc, true);
        await _page.WaitForTimeoutAsync(500);
        await productionShiftGroupPage.searchFor(shiftName);

        Assert.IsTrue(await validateHelper.IsValuePresent(await productionShiftGroupPage.getListRecord(), "Name", shiftName));
        // Only name is unique, so we just check name to verify the record is created successfully, Description and ShiftTypes are not necessary to check because they are not unique and can be same with other record

            

    }
    [Test]
    public async Task AddNewProductionShiftGroupWithBlankName()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        var shiftName = "";
        var shiftDesc = generateHelper.GenerateRandomString(10, "Description_");
        await productionShiftGroupPage.addProductionShiftGroup(shiftName, shiftDesc, true);
        await _page.WaitForTimeoutAsync(500);
        
        Assert.IsTrue(await _page.GetByText("Name is required.").IsVisibleAsync());
        

            

    }
    [Test]
    public async Task AddNewProductionShiftGroupWithAlredyExistsName()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        var shiftName = "test";// Need to make sure this value is already exist :)))
        var shiftDesc = generateHelper.GenerateRandomString(10, "Description_");
        await productionShiftGroupPage.addProductionShiftGroup(shiftName, shiftDesc, true);
        await _page.WaitForTimeoutAsync(500);
        await productionShiftGroupPage.searchFor(shiftName);

       Assert.IsTrue(await _page.GetByText("Cannot add duplicate production shift group").IsVisibleAsync());

            

    }
    [Test]
    public async Task SearchwithValidValue()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        var value = "day";
        await productionShiftGroupPage.searchFor(value);
        

        Assert.IsTrue(await validateHelper.IsValuePresent(await productionShiftGroupPage.getListRecord(), "Name",value)||
            await validateHelper.IsValuePresent(await productionShiftGroupPage.getListRecord(), "Description",value)||
            await validateHelper.IsValuePresent(await productionShiftGroupPage.getListRecord(), "ShiftTypes",value));
            // record will be shown if either Name or Description or ShiftTypes contains the search value
    }
    [Test]
    public async Task SearchwithInvalidValue()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        var value = "Something does not exist";// Need to make sure this value is not exist :)))
        await productionShiftGroupPage.searchFor(value);
        

        Assert.IsTrue(await _page.GetByText("No matching records found").IsVisibleAsync());
            // record will be shown if either Name or Description contains the search value
    }
    [Test]
    public async Task EditInformationExistingActiveProductionShiftGroup()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        List<ProductionShiftGroupTestData> list = await productionShiftGroupPage.getListRecord();
        var existingRecord = list.FirstOrDefault(item => item.Name != null);

        await productionShiftGroupPage.selectFirstProductionShiftGroup();
        await productionShiftGroupPage.clickEditBtn();

        Assert.IsTrue(await productionShiftGroupPage.getInitialStatus());

        productionShiftGroupPage.editProductionShiftGroup(existingRecord.Name + "_edited", existingRecord.Description + "_edited", true);
        await _page.WaitForTimeoutAsync(500);
        await productionShiftGroupPage.searchFor(existingRecord.Name + "_edited");
        Assert.IsTrue(await validateHelper.IsValuePresent(await productionShiftGroupPage.getListRecord(), "Name", existingRecord.Name + "_edited"));

    }
    [Test]
    public async Task EditInformationExistingInActiveProductionShiftGroup()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        await productionShiftGroupPage.showInactive();
        List<ProductionShiftGroupTestData> list = await productionShiftGroupPage.getListRecord();
        var existingRecord = list.FirstOrDefault(item => item.Name != null);

        await productionShiftGroupPage.selectFirstProductionShiftGroup();
        await productionShiftGroupPage.clickEditBtn();

        Assert.IsFalse(await productionShiftGroupPage.getInitialStatus());

        productionShiftGroupPage.editProductionShiftGroup(existingRecord.Name + "_edited", existingRecord.Description + "_edited", true);
        await _page.WaitForTimeoutAsync(500);
        await productionShiftGroupPage.searchFor(existingRecord.Name + "_edited");
        Assert.IsTrue(await validateHelper.IsValuePresent(await productionShiftGroupPage.getListRecord(), "Name", existingRecord.Name + "_edited"));

    }
    [Test]
    public async Task EditStatusOfExistingProductionShiftGroup()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        
        List<ProductionShiftGroupTestData> list = await productionShiftGroupPage.getListRecord();
        var existingRecord = list.FirstOrDefault(item => item.Name != null);

        await productionShiftGroupPage.selectFirstProductionShiftGroup();
        await productionShiftGroupPage.clickEditBtn();

        await productionShiftGroupPage.editProductionShiftGroup(existingRecord.Name, existingRecord.Description , false);
        await productionShiftGroupPage.showInactive();
        await productionShiftGroupPage.searchFor(existingRecord.Name);

        Assert.IsTrue(await validateHelper.IsValuePresent(await productionShiftGroupPage.getListRecord(), "Name", existingRecord.Name));

        await productionShiftGroupPage.selectFirstProductionShiftGroup();
        await productionShiftGroupPage.clickEditBtn();
        await productionShiftGroupPage.editProductionShiftGroup(existingRecord.Name, existingRecord.Description , true);
        await productionShiftGroupPage.showInactive();
        await productionShiftGroupPage.searchFor(existingRecord.Name);

        Assert.IsTrue(await validateHelper.IsValuePresent(await productionShiftGroupPage.getListRecord(), "Name", existingRecord.Name));



       
    }

}