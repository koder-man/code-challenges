using Microsoft.Extensions.Configuration;

namespace AoCCore;

internal static class Data
{
    private static readonly IConfigurationRoot configurationRoot = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    private static readonly Lazy<string> AoCCookie = new(() => configurationRoot["cookies"] ?? throw new Exception("Cookie not specified"));

    private static readonly HttpClient _httpClient = new();

    public static Stream Read(int day)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://adventofcode.com/2024/day/{day}/input");

        requestMessage.Headers.Add("Cookie", AoCCookie.Value);

        var response = _httpClient.SendAsync(requestMessage).Result; // yup

        return response.Content.ReadAsStream();
    }
}