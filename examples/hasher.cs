using System;
using Microsoft.AspNetCore.Http;

namespace Fluxera.Functions
{
    public class hasher
    {
        public string execute(HttpRequest request)
        {
            return BCrypt.Net.BCrypt.HashPassword("hello world", 10);
        }
    }
}
