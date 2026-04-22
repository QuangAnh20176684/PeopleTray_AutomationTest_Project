using Microsoft.Playwright;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Allure.Net.Commons;
using Allure.NUnit;


namespace PeoTest;

[TestFixture]
[AllureNUnit]
public class AddNewMovementTest : CommonBaseTest
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
        await productionPage.SwitchToOptionsLeftMenubar("Add New...", "Movement");


    }
    [Test]
    public async Task Test()
    {
        AddNewMovementPage addNewMovementPage = new AddNewMovementPage(_page);
        await addNewMovementPage.selectSite("Transfer");
        await _page.WaitForTimeoutAsync(5000);

    }
    [Test]
    public async Task selectMovementType()
    // no equip, no material, just LTS
    {
        AddNewMovementPage addNewMovementPage = new AddNewMovementPage(_page);
        await addNewMovementPage.selectMovementType("Andy_MT_LTS");
        await _page.WaitForTimeoutAsync(500);
        Assert.IsTrue(await _page.GetByText("No Material Type").IsVisibleAsync() &&
         await _page.Locator("#select2-chosen-5").IsVisibleAsync() &&
         await _page.Locator("#select2-chosen-3").IsVisibleAsync());
    }
    [Test]
    public async Task selectMovementType1()
    // no equip, material= fresh waste, STS
    {
        AddNewMovementPage addNewMovementPage = new AddNewMovementPage(_page);
        await addNewMovementPage.selectMovementType("Andy_MT_FixedMT_STS");
        await _page.WaitForTimeoutAsync(500);
        Assert.IsTrue(
         await _page.Locator("#select2-chosen-2").IsVisibleAsync() &&
         await _page.Locator("#select2-chosen-3").IsVisibleAsync());
        Assert.AreEqual(await _page.Locator("#dispMovementTypeMaterial").InnerTextAsync(), "Fresh Waste");
        Assert.AreEqual(await _page.Locator("#select2-chosen-7").InnerTextAsync(), "Fresh Waste");
    }
    [Test]
    public async Task selectMovementType2()
    // equip, no material, LTS
    {
        AddNewMovementPage addNewMovementPage = new AddNewMovementPage(_page);
        await addNewMovementPage.selectMovementType("Andy_MT_Equipment_LTS");
        await _page.WaitForTimeoutAsync(500);
        Assert.IsTrue(await _page.GetByText("No Material Type").IsVisibleAsync() &&
         await _page.Locator("#select2-chosen-5").IsVisibleAsync() &&
         await _page.Locator("#select2-chosen-3").IsVisibleAsync());
        Assert.IsTrue(await _page.Locator("#select2-chosen-6").IsVisibleAsync());
    }
    [Test]
    public async Task selectSite1()
    // site without Project
    {
        AddNewMovementPage addNewMovementPage = new AddNewMovementPage(_page);
        await addNewMovementPage.selectSite("Andy_site_ProX");
        await _page.WaitForTimeoutAsync(500);
        Assert.AreEqual(await _page.Locator("#select2-chosen-9").InnerTextAsync(), "Search Project");

    }
    [Test]
    public async Task selectSite2()
    // site with linked Project
    {
        AddNewMovementPage addNewMovementPage = new AddNewMovementPage(_page);
        await addNewMovementPage.selectSite("Andy_site_ProV");
        await _page.WaitForTimeoutAsync(500);
        Assert.AreEqual(await _page.Locator("#select2-chosen-9").InnerTextAsync(), "Andy_test_project");

    }
    [Test]
    public async Task selectSite3()
    // site without linked shift_group
    {
        AddNewMovementPage addNewMovementPage = new AddNewMovementPage(_page);
        await addNewMovementPage.selectSite("Andy_site_shiftS");
        await _page.WaitForTimeoutAsync(500);
        await _page.GetByText("Search Shift Group").ClickAsync();
        Assert.AreEqual(3, await _page.Locator(".select2-results").Locator("//li[@role='presentation']").CountAsync());

    }
    [Test]
    public async Task selectMovementType4()
    // Has UNIT, defined number of loads
    {
        AddNewMovementPage addNewMovementPage = new AddNewMovementPage(_page);
        await addNewMovementPage.selectMovementType("Andy_MT_FixedMT_STS");
        await _page.WaitForTimeoutAsync(500);


        Assert.AreEqual("123", await _page.Locator("#NumberOfLoads").InputValueAsync());
        Assert.AreEqual(await _page.Locator("#spnUnits").InnerTextAsync(), "BCM");
    }


}