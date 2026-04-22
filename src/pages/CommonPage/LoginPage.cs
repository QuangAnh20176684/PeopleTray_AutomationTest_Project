using Microsoft.Playwright;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PeoTest;

public class LoginPage : CommonBasePage
{
    public LoginPage(IPage page) : base(page) { }

    // =========================
    // 🔹 SELECTOR
    // =========================
    private ILocator EmailInput => _page.Locator("input[id='UserName']");
    private ILocator PasswordInput => _page.Locator("input[id='Password']");
    private ILocator LoginButton => _page.Locator("button[id='btnSubmit']");

    // =========================
    // 🔹 ACTIONS
    // =========================
    public async Task Login(string email, string password)
    {
        await Navigate(CT_URL.Base);
        Console.WriteLine("Perform login...");

        await EmailInput.FillAsync(email);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }
}
