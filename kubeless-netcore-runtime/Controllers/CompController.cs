using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using kubeless_netcore_runtime.Util;
using System.Collections.Generic;

namespace kubeless_netcore_runtime.Controllers
{
    [Produces("application/json")]
    [Route("api/Comp")]
    public class CompController : Controller
    {
        // GET: api/Comp
        [HttpPost]
        public object Post([FromBody]object data)
        {
            var code = System.IO.File.ReadAllText(@"../examples/simple.cs");

            var className = "CustomClass";
            var functionName = "Execute";

            var objeto = (JObject)data;
            string what = (string)objeto["message"];

            var compiler = new Compiler(code, className, functionName);
            var result = compiler.Start();
            if (result)
                return compiler.Execute(new object[] { HttpContext.Request});
            else
                return compiler.GetErrors();
        }
        
    }
}
