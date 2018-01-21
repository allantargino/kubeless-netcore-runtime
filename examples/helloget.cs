using System;
using Microsoft.AspNetCore.Http;

public class helloget
{
    public string execute(HttpRequest request)
    {
        return "hello world";
    }
}