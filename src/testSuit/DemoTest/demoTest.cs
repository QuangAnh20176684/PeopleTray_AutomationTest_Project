using Allure.NUnit;
using Microsoft.Playwright;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeoTest;

[TestFixture]
[AllureNUnit]
public class DemoTest : CommonBaseTest
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
        await _page.WaitForTimeoutAsync(2000);


    }
    [Test]
    public async Task Test()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        // await stockCataloguePage.combinedFilter("Fuel", "A", "001");
        // await stockCataloguePage.Search();
        // await _page.WaitForTimeoutAsync(500);
        List<StockCatalogueTestData> stockCatalogueList = await stockCataloguePage.getListRecord();
        foreach (var item in stockCatalogueList)
        {
            Console.WriteLine($"Category: {item.Category}, Name: {item.Name}, Unit: {item.Unit}, Cost: {item.Cost}, PartNum: {item.PartNum}, QRcode: {item.QRcode}, Description: {item.Description}, SubType: {item.SubType}");

        }
        // bool result = await validateHelper.IsValuePresent(stockCatalogueList, "Category", "Fuel");
    }
    [Test]
    public async Task AddStockItemWithValidData()
    
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        // Validate the new stock item appears in the list
        await stockCataloguePage.combinedFilter("Fuel", randomName, "PC001");
        await stockCataloguePage.Search();

        Assert.IsTrue(await validateHelper.IsValuePresent(await stockCataloguePage.getCategoryList(), "Fuel")
            && await validateHelper.IsValuePresent(await stockCataloguePage.getNameList(), randomName)
            && await validateHelper.IsValuePresent(await stockCataloguePage.getPartNumList(), "PC001"));


    }
    [Test]
    public async Task EditCostOfActiveStock_NegativeCost()
    // to <0 value

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "123", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(3000);
        await stockCataloguePage.nameFilter(randomName);
        await _page.WaitForTimeoutAsync(3000);
        await stockCataloguePage.Search();
        await _page.WaitForTimeoutAsync(3000);
        await stockCataloguePage.editInformationOfStockItem("Cost", "-123", true, true);
        await _page.WaitForTimeoutAsync(5000);

        Assert.IsTrue(await _page.GetByText("Cost must be a positive number.").IsVisibleAsync());



    }
    
    
    // [Test]
    // public async Task EditPartNumberOfActiveStock_ValidPartNumber()
    // // to valid value

    // {
    //     StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
    //     await _page.GetByText("bruhhh").ClickAsync();
    //     var randomName = generateHelper.GenerateRandomString(10);
    //     var newPartNum = generateHelper.GenerateRandomString(20, "EditedPNumber_");
    //     await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
    //     await _page.WaitForTimeoutAsync(500);
    //     await stockCataloguePage.nameFilter(randomName);
    //     await stockCataloguePage.Search();
    //     await stockCataloguePage.editInformationOfStockItem("PartNum", newPartNum, true, true);


    //     await stockCataloguePage.nameFilter(randomName);
    //     await stockCataloguePage.Search();

    //     Assert.IsTrue(Equals(newPartNum, await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[4]").InnerTextAsync()));


    // }
    }
    