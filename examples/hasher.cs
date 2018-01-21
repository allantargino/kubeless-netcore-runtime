using System;
using Microsoft.AspNetCore.Http;

namespace Fluxera.Functions
{
    public class Hasher
    {
        public string Execute(HttpRequest request)
        {
            return BCrypt.Net.BCrypt.HashPassword("hello world", 10);
        }
    }
}
