namespace Kubeless.WebAPI.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Kubeless.Core.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    [Route("/")]
    public class RuntimeController : Controller
    {
        private readonly IFunction function;
        private readonly IInvoker invoker;

        public RuntimeController(IFunction function, IInvoker invoker)
        {
            this.function = function;
            this.invoker = invoker;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]object data)
        {
            // TODO: Create custom context (dynamic?) from Request.

            if (Request.Body.CanSeek)
            {
                Request.Body.Position = 0;
            }
            
            object result = await this.invoker.Execute(this.function, Request);
            return this.GetSuitableActionResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            object result = await this.invoker.Execute(this.function, Request);

            stopwatch.Stop();
            System.Console.WriteLine($"Function invokation took {stopwatch.ElapsedTicks} ticks.");

            return this.GetSuitableActionResult(result);
        }

        [HttpGet("/healthz")]
        public IActionResult Health() => this.Ok();    

        private IActionResult GetSuitableActionResult(object result)
        {
            if(result is string) 
            {
                return this.Ok(result);
            }

            if(result == null)
            {
                return this.Ok();
            }

            return this.Json(result);
        }          
    }
}
