using System;
using Microsoft.AspNetCore.Http;

public class mycodejson
{
    public object execute(HttpRequest request)
    {
        return new 
        { 
            text = "hello world"
        };
    }
}