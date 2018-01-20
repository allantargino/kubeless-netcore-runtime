using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class mycodeasync
{
    public async Task<string> execute(HttpRequest request)
    {
        return await Task.FromResult("hello async world");
    }
}
