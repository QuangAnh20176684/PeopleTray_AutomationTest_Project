using Microsoft.Playwright;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeoTest;

[TestFixture]

public class DrillbitsTest : CommonBaseTest
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
        await productionPage.SwitchToOptionsLeftMenubar("Drill Bits");


    }
    [Test]
    public async Task Test()
    {
        DrillbitsPage drillbitsPage = new DrillbitsPage(_page);
        await drillbitsPage.FilterEquipment("Test");
        await _page.WaitForTimeoutAsync(3000);
    }

    [Test]
    public async Task TestFilterByBitNumberExits()
    {
        // Arrange
        DrillbitsPage drillbitsPage = new DrillbitsPage(_page);
        string bitNumber = "test";

        // Act
        await drillbitsPage.FilterBitnum(bitNumber);
        await drillbitsPage.Search();
        


        // Assert
        
        
        Assert.IsTrue(await validateHelper.IsValuePresent(await drillbitsPage.GetBitnumberList(), bitNumber));
        


    }
    [Test]
    public async Task TestFilterByBitNumberNotExits()
    {
        // Arrange
        DrillbitsPage drillbitsPage = new DrillbitsPage(_page);
        string bitNumber = "Craack";

        // Act
        await drillbitsPage.FilterBitnum(bitNumber);
        await drillbitsPage.Search();

        // Assert
        Assert.IsTrue(await validateHelper.IsNodataPresent(await drillbitsPage.GetBitnumberList()));
    }
    


    [Test]
    public async Task TestFilterByEquipmentExits()
    {
        // Arrange
        DrillbitsPage drillbitsPage = new DrillbitsPage(_page);
        string equipmentName = "CV Test 101";

        // Act
        await drillbitsPage.FilterEquipment(equipmentName);
        await drillbitsPage.Search();

        // Assert
        Assert.IsTrue(await validateHelper.IsValuePresent(await drillbitsPage.GetEquipmentList(), equipmentName));
        
    }
    [Test]
    public async Task TestFilterByEquipmentNotExits()
    {
        // Arrange
        DrillbitsPage drillbitsPage = new DrillbitsPage(_page);
        string equipmentName = "Craack";

        // Act
        await drillbitsPage.FilterEquipment(equipmentName);
        

        // Assert
        Assert.IsTrue(await _page.Locator("//ul[@id='select2-results-1']").GetByText("No matches found").IsVisibleAsync());
        
    }

    [Test]
    public async Task TestFilterByInvalidDateRange()
    //Start date > end date
    {
        // Arrange
        DrillbitsPage drillbitsPage = new DrillbitsPage(_page);
        string startDate = "10-Mar-2025";
        string endDate = "1-Mar-2025";

        // Act
        await drillbitsPage.FilterStartDate(startDate, endDate);
        await drillbitsPage.Search();

        // Assert
        Assert.IsTrue(await validateHelper.IsNodataPresent(await drillbitsPage.GetStartDateList()));
    }
    [Test]
    public async Task TestFilterByValidDateRange()
    {
        // Arrange
        DrillbitsPage drillbitsPage = new DrillbitsPage(_page);
        string startDate = "1-Mar-2025";
        string endDate = "10-Mar-2025";

        // Act
        await drillbitsPage.FilterStartDate(startDate, endDate);
        await drillbitsPage.Search();

        // Assert
        
    }


    [Test]
    public async Task TestClearFilters()
    {
        // Arrange
        DrillbitsPage drillbitsPage = new DrillbitsPage(_page);

        // Act
        await drillbitsPage.ClearFilters();

        // Assert
        // Add assertions to verify that filters are cleared
    }

    [Test]
    public async Task TestPagination()
    {
        // Arrange
        DrillbitsPage drillbitsPage = new DrillbitsPage(_page);

        // Act
        await drillbitsPage.NextPage("Next");
        await drillbitsPage.PreviousPage("Previous");

        // Assert
        // Add assertions to verify pagination works correctly
    }

    [Test]
    public async Task TestCombinedFilters()
    {
        // Arrange
        DrillbitsPage drillbitsPage = new DrillbitsPage(_page);
        string bitNumber = "12345";
        string equipmentName = "Drill Machine A";
        string startDate = "2026-01-01";
        string endDate = "2026-12-31";

        // Act
        await drillbitsPage.FilterBitnum(bitNumber);
        await drillbitsPage.FilterEquipment(equipmentName);
        await drillbitsPage.FilterStartDate(startDate, endDate);
        await drillbitsPage.Search();

        // Assert
        // Add assertions to verify the combined filtered results
    }
}
