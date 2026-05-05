using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;



namespace PeoTest;

[TestFixture]

public class CommonBaseTest
{
    protected IPage _page;
    protected IBrowser _browser;
    protected IBrowserContext _context;
    protected IPlaywright _playwright;

    [SetUp]
    public async Task Setup()
    {
        // // Initialize Allure
        // AllureLifecycle.Instance.StartTestCase(TestContext.CurrentContext.Test.FullName);
        
        _playwright = await Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless =  true, // ✅ chạy ngầm
            SlowMo = 0
        });

        _context = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            // ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
            ViewportSize = null
        });

        // ✅ Bật trace
        // await _context.Tracing.StartAsync(new TracingStartOptions
        // {
        //     Screenshots = true,
        //     Snapshots = true,
        //     Sources = true
        // });

        _page = await _context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        // // ✅ Lưu trace ra file
        // await _context.Tracing.StopAsync(new TracingStopOptions
        // {
        //     Path = "trace.zip"
        // });

        // await _context.CloseAsync();
        await _browser.CloseAsync();
        _playwright.Dispose();
        
        // // Stop Allure test
        // AllureLifecycle.Instance.StopTestCase();
        // AllureLifecycle.Instance.WriteTestCase();
    }
}
