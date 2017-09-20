using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace kubeless_netcore_runtime.Controllers
{
    [Route("/")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return "Hey, CSE Rocks!";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

    }
}
