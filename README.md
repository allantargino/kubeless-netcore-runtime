# Kubeless .NET Core Runtime

This is a .NET Core 2.0 runtime for the serverless functions framework [Kubeless](https://github.com/kubeless/kubeless). The functions are provided as source code and are compiled when the docker container (a k8s pod) for the function is initialized.

## Http Functions

Functions triggered by the http-trigger can be implemented using the async/await keywords, but this is not mandatory. By supporting async/await you are able to use APIs returning a Task/Task<T>.

## Create a function

To create a function simply execute `dotnet new classlib -o MyFunction` to create a `netstandard2.0` class library project. Change the name of the generated default class `Class1` to something useful. The name of the class ist the name of your new function. You can use namespaces and add NuGet packages to the `*.csproj` file like you would with any other class libary roject.

You can name the function handler method anything you like, but the method must declare one parameter: the `HttpContext` of the http request. The method can return any value you like. 

``'csharp
namespace Functions
{
    using System;
    using Microsoft.AspNetCore.Http;

    public class HelloWorld
    {
        public string Execute(HttpRequest request)
        {
            return "Hello World!";
        }
    }
}
```

## Supported Function Triggers

* :+1: HTTP-trigger
* PubSub-trigger (coming soon)
* Cron-trigger (coming soon)
