using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class mycodeasync
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<string> execute(HttpRequest request)
    {
        HttpResponseMessage result = await client.GetAsync("https://raw.githubusercontent.com/DXBrazil/kubeless-netcore-runtime/master/README.md");
        return await result.Content.ReadAsStringAsync();
    }
}
