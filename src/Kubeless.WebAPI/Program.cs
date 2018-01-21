namespace Kubeless.WebAPI
{
    using System;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            string envPortStr = Environment.GetEnvironmentVariable("FUNC_PORT");
            string port = string.IsNullOrWhiteSpace(envPortStr) ? "8080" : envPortStr;

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://*:{port}")
                .Build();
        }
    }
}
