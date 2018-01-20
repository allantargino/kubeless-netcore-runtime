namespace Kubeless.WebAPI.Controllers
{
    using Kubeless.WebAPI.Utils;
    using Microsoft.AspNetCore.Mvc;

    [Route("/report")]
    public class ReportController : Controller
    {
        private readonly ReportBuilder reportBuilder;

        public ReportController(ReportBuilder reportBuilder)
        {
            this.reportBuilder = reportBuilder;
        }

        [HttpGet]
        public string FunctionReport()
        {
            return this.reportBuilder.GetReport();
        }
    }
}
