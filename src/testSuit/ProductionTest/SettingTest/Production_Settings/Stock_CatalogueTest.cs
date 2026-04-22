using Microsoft.Playwright;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeoTest;

[TestFixture]

public class Stock_CatalogueTest : CommonBaseTest
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
    // need in active record check
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
    public async Task AddStockItemWithBlankName()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        await stockCataloguePage.addStockItem("Fuel", "", "Days", "100", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(300);
        // Validate error message is displayed
        Assert.IsTrue(await _page.GetByText("This field is required.").IsVisibleAsync());
    }
    [Test]
    public async Task AddStockItemWithBlankUnits()
    {

        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "", "100", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        // Validate error message is displayed
        Assert.IsTrue(await _page.GetByText("This field is required.").IsVisibleAsync());
    }
    [Test]
    public async Task AddStockItemWithNonExistingUnits()
    {

        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Invalid Unit", "100", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        // Validate error message is displayed
        Assert.IsTrue(await _page.GetByText("No matches found").IsVisibleAsync());
    }
    [Test]
    public async Task AddStockItemWithBlankCost()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        // Validate error message is displayed
        Assert.IsTrue(await _page.GetByText("This field is required.").IsVisibleAsync());
    }
    [Test]
    public async Task AddStockItemWithCostContainsNonNumericCharacters()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "invalid", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        // Validate error message is displayed
        Assert.IsTrue(await _page.GetByText("Please enter a valid number.").IsVisibleAsync());
    }
    [Test]
    public async Task AddStockItemWithCostSmallerThan0()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        // Validate error message is displayed
        Assert.IsTrue(await _page.GetByText("Cost must be a positive number.").IsVisibleAsync());
    }
    [Test]
    public async Task AddStockItemWithNameAlreadyExistsSameStatus()
    //same status
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.addStockItem("Consumable", randomName, "Hours", "100", "PC002", "1234567890124", "High quality sand", true);
        // Validate error message is displayed
        Assert.IsTrue(await _page.GetByText($"{randomName} already exists.").IsVisibleAsync());
    }
    [Test]
    public async Task AddStockItemWithNameAlreadyExistsDifferentStatus()
    //different status
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.addStockItem("Consumable", randomName, "Hours", "100", "PC002", "1234567890124", "High quality sand", false);
        // Validate error message is displayed
        Assert.IsTrue(await _page.GetByText($"{randomName} already exists.").IsVisibleAsync());


    }
    [Test]
    public async Task DeleteActiveStock()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();



        await stockCataloguePage.Delete();

        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();


        Assert.IsTrue(await _page.GetByText("No data available in table").IsVisibleAsync());



    }
    [Test]
    public async Task DeleteInactiveStock()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", false);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.displayInactive(true);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();



        await stockCataloguePage.Delete();

        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();


        Assert.IsTrue(await _page.GetByText("No data available in table").IsVisibleAsync());



    }
    [Test]
    public async Task EditNameOfActiveStock_ValidName()
    // valid name
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        var newName = generateHelper.GenerateRandomString(10, "EditedName_");
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Name", newName, true, true);

        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        Assert.IsTrue(await _page.GetByText("No data available in table").IsVisibleAsync());

    }
    [Test]
    public async Task EditNameOfActiveStock_BlankName()
    // blank name
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Name", "", true, true);


        Assert.IsTrue(await _page.GetByText("This field is required.").IsVisibleAsync());

    }
    [Test]
    public async Task EditNameOfActiveStock_ExistingName()
    // name alredy exist
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        List<StockCatalogueTestData> list = await stockCataloguePage.getListRecord();
        var existingName = list.FirstOrDefault(item => item.Name != null).Name;
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Name", existingName, true, true);


        Assert.IsTrue(await _page.GetByText($"{existingName} already exists.").IsVisibleAsync());

    }
    [Test]
    public async Task EditCategoryOfActiveStock_ValidCategory()

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Category", "Consumable", true, true);

        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        List<StockCatalogueTestData> list = await stockCataloguePage.getListRecord();
        Assert.IsTrue(Equals("Consumable", await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[2]").InnerTextAsync()));

    }
    [Test]
    public async Task EditUnitsOfActiveStock_ValidUnit()
    // to null

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Unit", "", true, true);


        Assert.IsTrue(await _page.GetByText("This field is required.").IsVisibleAsync());

    }
    [Test]
    public async Task EditUnitsOfActiveStock_AnotherUnit()
    // to another options


    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Unit", "Hours", true, true);


        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        List<StockCatalogueTestData> list = await stockCataloguePage.getListRecord();
        Assert.IsTrue(Equals("Hours", await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[5]").InnerTextAsync()));

    }
    [Test]
    public async Task EditUnitsOfActiveStock_NonExistingUnit()// could run in flarky sometimes
                                               // to non-existing options


    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Unit", "InvalidValue", true, true);
        await _page.WaitForTimeoutAsync(1000);

        Assert.IsTrue(await _page.GetByText("No matches found").IsVisibleAsync());

    }
    [Test]
    public async Task EditCostOfActiveStock_ValidCost()
    // to valid value

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Cost", "123", true, true);


        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        List<StockCatalogueTestData> list = await stockCataloguePage.getListRecord();
        Assert.IsTrue(Equals("123", await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[6]").InnerTextAsync()));


    }
    [Test]
    public async Task EditCostOfActiveStock_InvalidCost()
    // to string contains text

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        var randomstring = generateHelper.GenerateRandomString(20, "Editedcost_");
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Cost", randomstring, true, true);

        Assert.IsTrue(await _page.GetByText("Please enter a valid number.").IsVisibleAsync());




    }
    [Test]
    public async Task EditCostOfActiveStock_NegativeCost()
    // to <0 value

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Cost", "123", true, true);

        Assert.IsTrue(await _page.GetByText("Cost must be a positive number.").IsVisibleAsync());




    }
    [Test]
    public async Task EditCostOfActiveStock_NullCost()
    // to null

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Cost", "", true, true);

        Assert.IsTrue(await _page.GetByText("This field is required.").IsVisibleAsync());




    }
    [Test]
    public async Task EditPartNumberOfActiveStock_ValidPartNumber()
    // to valid value

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        var newPartNum = generateHelper.GenerateRandomString(20, "EditedPNumber_");
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("PartNum", newPartNum, true, true);


        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();

        Assert.IsTrue(Equals(newPartNum, await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[4]").InnerTextAsync()));


    }
    [Test]
    public async Task EditPartNumberOfActiveStock_NullPartNumber()
    // to null value

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("PartNum", "", true, true);


        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();

        Assert.IsTrue(Equals("", await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[4]").InnerTextAsync()));


    }
    [Test]
    public async Task EditQRCodeOfActiveStock1()
    // skip these next two tests because the QR code column is hidden 

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        var newQrCode= generateHelper.GenerateRandomString(20,"EditedQrcode_");
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("QRcode", newQrCode, true, true);


        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();

        Assert.IsTrue(Equals(newQrCode, await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[4]").InnerTextAsync()));


    }
    [Test]
    public async Task EditQrCodeOfActiveStock2()
    // to null value

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("QRcode", "", true, true);


        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();

        Assert.IsTrue(Equals("", await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[4]").InnerTextAsync()));


    }
    [Test]
    public async Task EditDescriptionOfActiveStock_ValidDescription()
    // to valid value

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        var newDesc = generateHelper.GenerateRandomString(20, "EditedDesc_");
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Description", newDesc, true, true);


        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();

        Assert.IsTrue(Equals(newDesc, await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[7]//a[@rel='tooltip']").GetAttributeAsync("data-original-title")));


    }
    [Test]
    public async Task EditDescriptionOfActiveStock_NullDescription()
    // to null value

    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Description", "", true, true);


        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();

        Assert.IsTrue(Equals("", await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[7]").InnerTextAsync()));


    }
    [Test]
    public async Task EditStatusOfInActiveStock()
    // To active
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", false);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.displayInactive(true);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();

        await stockCataloguePage.editInformationOfStockItem("Status", "Active", false, true);

        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        Assert.IsTrue(await _page.GetByText("No data available in table").IsVisibleAsync());


    }
    [Test]
    public async Task EditStatusOfActiveStock()
    // To active
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);

        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "-10", "PC001", "1234567890123", "High quality cement", true);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Status", "Active", true, false);

        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        Assert.IsTrue(await _page.GetByText("No data available in table").IsVisibleAsync());


    }

    // ============================================
    // 🔹 FILTER & SEARCH TESTS
    // ============================================

    [Test]
    public async Task SearchWithCategoryFilterOnly_DisplaysCorrectResults()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        await stockCataloguePage.categoryFilter("Fuel");
        await stockCataloguePage.Search();

        var categoryList = await stockCataloguePage.getCategoryList();
        Assert.IsTrue(categoryList.Count > 0 && categoryList.All(cat => cat == "Fuel"));
    }

    [Test]
    public async Task SearchWithNameFilterOnly_DisplaysCorrectResults()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();

        var nameList = await stockCataloguePage.getNameList();
        Assert.IsTrue(await validateHelper.IsValuePresentExactly(nameList, randomName));
    }

    [Test]
    public async Task SearchWithPartNumberFilterOnly_DisplaysCorrectResults()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC123", "1111111111111", "Test item", true);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.partNumFilter("PC123");
        await stockCataloguePage.Search();

        var partNumList = await stockCataloguePage.getPartNumList();
        Assert.IsTrue(partNumList.All(p => p == "PC123"));
    }

    [Test]
    public async Task SearchWithCombinedFilters_DisplaysOnlyMatchingResults()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC123", "1111111111111", "Test item", true);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.combinedFilter("Fuel", randomName, "PC123");
        await stockCataloguePage.Search();

        Assert.IsTrue(await validateHelper.IsValuePresent(await stockCataloguePage.getNameList(), randomName));
    }

    [Test]
    public async Task ResetFilter_ClearsAllFiltersAndShowsAllResults()
    // cannot clear category filter
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        await stockCataloguePage.combinedFilter("Fuel", "TestName", "PC999");
        await stockCataloguePage.resetFilter();
        await _page.WaitForTimeoutAsync(2000);

        var categoryValue = await _page.Locator("//select[@id='ddlConsumableCategory']").InputValueAsync();
        var nameValue = await _page.Locator("//input[@id='txtName']").InputValueAsync();
        var partNumValue = await _page.Locator("//input[@id='Para_PartNumber']").InputValueAsync();

        Assert.IsTrue(string.IsNullOrEmpty(categoryValue) && string.IsNullOrEmpty(nameValue) && string.IsNullOrEmpty(partNumValue));
    }

    [Test]
    public async Task DisplayInactiveCheckbox_TogglesInactiveItems()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Inactive item", false);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        Assert.IsTrue(await _page.GetByText("No data available in table").IsVisibleAsync());

        await stockCataloguePage.displayInactive(true);
        await stockCataloguePage.Search();
        Assert.IsTrue(await validateHelper.IsValuePresent(await stockCataloguePage.getNameList(), randomName));
    }

    // ============================================
    // 🔹 PART NUMBER EDIT TESTS
    // ============================================

    [Test]
    public async Task EditPartNumberOfActiveStock_WithSpecialCharacters()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        var specialPartNum = "PC-001!@#";

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("PartNum", specialPartNum, true, true);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        Assert.IsTrue(Equals(specialPartNum, await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[4]").InnerTextAsync()));
    }

    [Test]
    public async Task EditPartNumberOfActiveStock_MaxLengthValue()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        var maxPartNum = generateHelper.GenerateRandomString(100, "MaxPartNum_");

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("PartNum", maxPartNum, true, true);

        await _page.WaitForTimeoutAsync(500);
    }

    // ============================================
    // 🔹 QR CODE EDIT TESTS
    // ============================================

    [Test]
    public async Task EditQRCodeOfActiveStock_WithSpecialCharacters()
    // skip this test
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        var specialQRCode = "QR!@#$%^&";

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("QRcode", specialQRCode, true, true);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        Assert.IsTrue(Equals(specialQRCode, await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[4]").InnerTextAsync()));
    }

    [Test]
    public async Task EditQRCodeOfActiveStock_MaxLength()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        var maxQRCode = generateHelper.GenerateRandomString(150, "QR_");

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("QRcode", maxQRCode, true, true);

        await _page.WaitForTimeoutAsync(500);
    }

    // ============================================
    // 🔹 DESCRIPTION EDIT TESTS
    // ============================================

    [Test]
    public async Task EditDescriptionOfActiveStock_MaxLength()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        var maxDesc = generateHelper.GenerateRandomString(300, "VeryLongDescription_");

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Description", maxDesc, true, true);

        await _page.WaitForTimeoutAsync(500);
    }

    [Test]
    public async Task EditDescriptionOfActiveStock_WithSpecialCharacters()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        var specialDesc = "Test!@#$%^&*()_+-=";

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Description", specialDesc, true, true);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        var result = await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[7]//a[@rel='tooltip']").GetAttributeAsync("data-original-title");
        Assert.IsTrue(result.Equals(specialDesc));
    }

    // ============================================
    // 🔹 CATEGORY EDIT TESTS
    // ============================================

    [Test]
    public async Task EditCategoryOfInactiveStock_ValidCategory()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", false);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.displayInactive(true);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Category", "Consumable", false, false);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.displayInactive(true);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        Assert.IsTrue(Equals("Consumable", await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[2]").InnerTextAsync()));
    }

    // ============================================
    // 🔹 STATUS CHANGE TESTS
    // ============================================

    [Test]
    public async Task EditStatusOfInActiveStock_ToInactive()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", false);
        await _page.WaitForTimeoutAsync(500);
        await stockCataloguePage.displayInactive(true);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();

        await stockCataloguePage.editInformationOfStockItem("Status", "Inactive", false, false);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.displayInactive(true);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        Assert.IsTrue(await validateHelper.IsValuePresent(await stockCataloguePage.getNameList(), randomName));
    }

    // ============================================
    // 🔹 ADD STOCK EDGE CASES TESTS
    // ============================================

    [Test]
    public async Task AddStockItemWithNameMaxLength()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var maxName = generateHelper.GenerateRandomString(100, "VeryLongName_");

        await stockCataloguePage.addStockItem("Fuel", maxName, "Days", "100", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(maxName);
        await stockCataloguePage.Search();
        Assert.IsTrue(await validateHelper.IsValuePresent(await stockCataloguePage.getNameList(), maxName));
    }

    [Test]
    public async Task AddStockItemWithSpecialCharactersInName()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var specialName = generateHelper.GenerateRandomString(10) + "_!@#";

        await stockCataloguePage.addStockItem("Fuel", specialName, "Days", "100", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.combinedFilter("Fuel", specialName, "PC001");
        await stockCataloguePage.Search();
        Assert.IsTrue(await validateHelper.IsValuePresent(await stockCataloguePage.getNameList(), specialName));
    }

    [Test]
    public async Task AddStockItemWithCostZero()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "0", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.combinedFilter("Fuel", randomName, "PC001");
        await stockCataloguePage.Search();

        Assert.IsTrue(await validateHelper.IsValuePresent(await stockCataloguePage.getNameList(), randomName));
    }

    [Test]
    public async Task AddStockItemWithDecimalCost()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "99.99", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.combinedFilter("Fuel", randomName, "PC001");
        await stockCataloguePage.Search();

        var costValue = await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[6]").InnerTextAsync();
        Assert.IsTrue(costValue.Equals("99.99") || await validateHelper.IsValuePresentExactly(await stockCataloguePage.getNameList(), randomName));
    }

    [Test]
    public async Task AddStockItemWithVeryLargeCost()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "999999.99", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();

        Assert.IsTrue(await validateHelper.IsValuePresent(await stockCataloguePage.getNameList(), randomName));
    }

    // ============================================
    // 🔹 UNIT CHANGE TESTS
    // ============================================

    [Test]
    public async Task EditUnitsOfActiveStock_ChangeMultipleTimes()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", true);
        await _page.WaitForTimeoutAsync(2000);

        // Thay đổi lần đầu
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Unit", "Hours", true, true);
        await _page.WaitForTimeoutAsync(2000);

        // Thay đổi lần thứ hai
        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Unit", "Litres", true, true);
        await _page.WaitForTimeoutAsync(2000);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        Assert.IsTrue(Equals("Litres", await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[5]").InnerTextAsync()));
    }

    // ============================================
    // 🔹 MULTIPLE EDITS IN SEQUENCE
    // ============================================

    [Test]
    public async Task EditMultipleFieldsInSequence()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        var newName = generateHelper.GenerateRandomString(10, "Updated_");
        var newPartNum = generateHelper.GenerateRandomString(15, "NewPart_");

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Original", true);
        await _page.WaitForTimeoutAsync(500);

        // Edit Name
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Name", newName, true, true);
        await _page.WaitForTimeoutAsync(500);

        // Edit PartNum
        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(newName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("PartNum", newPartNum, true, true);
        await _page.WaitForTimeoutAsync(500);

        // Edit Description
        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(newName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Description", "Updated Description", true, true);

        // Verify all changes
        await stockCataloguePage.resetFilter();
        await stockCataloguePage.nameFilter(newName);
        await stockCataloguePage.Search();
        Assert.IsTrue(await validateHelper.IsValuePresent(await stockCataloguePage.getNameList(), newName));
    }

    // ============================================
    // 🔹 INACTIVE ITEM OPERATIONS
    // ============================================

    [Test]
    public async Task EditCostOfInactiveStock_ValidCost()
    // 250.50 is display as 250.5 in the table
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", false);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.displayInactive(true);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Cost", "250.50", false, false);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.displayInactive(true);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        Assert.IsTrue(Equals("250.50", await _page.Locator("//table[@id='tblConsumable']//tbody//tr[1]//td[6]").InnerTextAsync()));
    }

    [Test]
    public async Task EditNameOfInactiveStock_ValidName()
    {
        StockCataloguePage stockCataloguePage = new StockCataloguePage(_page);
        var randomName = generateHelper.GenerateRandomString(10);
        var newName = generateHelper.GenerateRandomString(10, "InactiveUpdated_");

        await stockCataloguePage.addStockItem("Fuel", randomName, "Days", "100", "PC001", "1234567890123", "Test", false);
        await _page.WaitForTimeoutAsync(500);

        await stockCataloguePage.displayInactive(true);
        await stockCataloguePage.nameFilter(randomName);
        await stockCataloguePage.Search();
        await stockCataloguePage.editInformationOfStockItem("Name", newName, false, false);

        await stockCataloguePage.resetFilter();
        await stockCataloguePage.displayInactive(true);
        await stockCataloguePage.nameFilter(newName);
        await stockCataloguePage.Search();
        Assert.IsTrue(await validateHelper.IsValuePresent(await stockCataloguePage.getNameList(), newName));
    }

}