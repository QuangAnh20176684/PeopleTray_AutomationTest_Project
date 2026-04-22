using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;


namespace PeoTest;

public class generateHelper
{
    public static string GenerateRandomString(int length)
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    var random = new Random();

    return new string(Enumerable.Range(0, length)
        .Select(_ => chars[random.Next(chars.Length)])
        .ToArray());
}
    
    public static string GenerateRandomString(int length, string prefix)
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    var random = new Random();

    string randomString = new string(Enumerable.Range(0, length)
        .Select(_ => chars[random.Next(chars.Length)])
        .ToArray());

    return prefix + randomString;
}
}