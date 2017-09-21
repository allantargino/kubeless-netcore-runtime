using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

public class CustomClass
{
    public int Execute(HttpRequest request)
    {
        var http = new HttpClient();
        var result = DoSomeMath(3,5);
        var list = new List<int>();
        list.Add(3);
        return result;
    }

    public int DoSomeMath(int x, int y) => x+y*10;
}