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

            //List files:
            var directory = "/bin";
            var files = System.IO.Directory.EnumerateFiles(directory);
            response.AppendLine($"FILES IN {directory}: {files.Count()}");

            try
            {
                //List files again:
                directory = "/kubeless/";
                files = System.IO.Directory.EnumerateFiles(directory);
                response.AppendLine($"FILES IN {directory}: {files.Count()}");
                foreach (var file in files)
                {
                    response.AppendLine($"FILE: {file}");
                    response.AppendLine("CONTENT:");
                    response.AppendLine(System.IO.File.ReadAllText(file));
                    response.AppendLine();
                }
            }
            catch (Exception ex)
            {
                response.AppendLine($"ERROR: {ex.Message}");
            }
            
            response.AppendLine("END");
            return response.ToString();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

    }
}
