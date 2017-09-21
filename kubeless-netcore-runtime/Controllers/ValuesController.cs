using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace kubeless_netcore_runtime.Controllers
{
    [Route("/")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            var response = new StringBuilder();

            response.AppendLine("Hey, CSE Rocks!");
            response.AppendLine($"MOD_NAME: {Environment.GetEnvironmentVariable("MOD_NAME")}");
            response.AppendLine($"FUNC_HANDLER: {Environment.GetEnvironmentVariable("FUNC_HANDLER")}");

            return response.ToString();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

    }
}
