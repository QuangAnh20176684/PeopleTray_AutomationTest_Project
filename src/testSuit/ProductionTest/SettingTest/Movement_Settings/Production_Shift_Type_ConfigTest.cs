using Microsoft.Playwright;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeoTest;

[TestFixture]

public class Production_Shift_Type_ConfigTest : CommonBaseTest
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
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        var shiftName = generateHelper.GenerateRandomString(5, "ShiftName_");
        var shiftDesc = generateHelper.GenerateRandomString(10, "Description_");
        await productionShiftGroupPage.addProductionShiftGroup(shiftName, shiftDesc, true);
        await productionShiftGroupPage.searchFor(shiftName);
        await productionShiftGroupPage.configureShiftTypes(shiftName);

        ProductionShiftTypesConfigPage productionShiftTypesConfigPage = new ProductionShiftTypesConfigPage(_page);
        productionShiftTypesConfigPage.addProductionShiftType("Dayshift");
        await _page.WaitForTimeoutAsync(5000);
        await productionShiftTypesConfigPage.selectShift("Dayshift");
        await _page.WaitForTimeoutAsync(5000);


    }
    [Test]
    public async Task TestAddNewShiftType()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        var shiftName = generateHelper.GenerateRandomString(5, "ShiftName_");
        var shiftDesc = generateHelper.GenerateRandomString(10, "Description_");
        await productionShiftGroupPage.addProductionShiftGroup(shiftName, shiftDesc, true);
        await productionShiftGroupPage.searchFor(shiftName);
        await productionShiftGroupPage.configureShiftTypes(shiftName);

        ProductionShiftTypesConfigPage productionShiftTypesConfigPage = new ProductionShiftTypesConfigPage(_page);
        productionShiftTypesConfigPage.addProductionShiftType("Dayshift");
        await productionShiftTypesConfigPage.searchForShiftType("Dayshift");

        
        Assert.IsTrue(await validateHelper.IsValuePresent(await productionShiftTypesConfigPage.getListRecord(), "ShiftTypeName", "Dayshift"));



    }
    [Test]
    public async Task TestAddNewBlankShiftType()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        var shiftName = generateHelper.GenerateRandomString(5, "ShiftName_");
        var shiftDesc = generateHelper.GenerateRandomString(10, "Description_");
        await productionShiftGroupPage.addProductionShiftGroup(shiftName, shiftDesc, true);
        await productionShiftGroupPage.searchFor(shiftName);
        await productionShiftGroupPage.configureShiftTypes(shiftName);

        ProductionShiftTypesConfigPage productionShiftTypesConfigPage = new ProductionShiftTypesConfigPage(_page);
        productionShiftTypesConfigPage.addProductionShiftType("");
        await _page.WaitForTimeoutAsync(5000);
        

        
        Assert.IsTrue(await _page.GetByText("Must select a shift type").IsVisibleAsync());



    }
    [Test]
    public async Task TestDeleteShiftType()
    {
        ProductionShiftGroupPage productionShiftGroupPage = new ProductionShiftGroupPage(_page);
        var shiftName = generateHelper.GenerateRandomString(5, "ShiftName_");
        var shiftDesc = generateHelper.GenerateRandomString(10, "Description_");
        await productionShiftGroupPage.addProductionShiftGroup(shiftName, shiftDesc, true);
        await productionShiftGroupPage.searchFor(shiftName);
        await productionShiftGroupPage.configureShiftTypes(shiftName);

        ProductionShiftTypesConfigPage productionShiftTypesConfigPage = new ProductionShiftTypesConfigPage(_page);
        productionShiftTypesConfigPage.addProductionShiftType("Dayshift");
        await productionShiftTypesConfigPage.searchForShiftType("Dayshift");
        await productionShiftTypesConfigPage.deleteShiftType("Dayshift");
        await productionShiftTypesConfigPage.searchForShiftType("Dayshift");

        
        Assert.IsTrue(await _page.GetByText("No data available in table").IsVisibleAsync());



    }


}