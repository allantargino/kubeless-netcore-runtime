namespace Kubeless.WebAPI.Utils
{
    using System;
    using System.Text;
    using Kubeless.Core.Interfaces;
    using Kubeless.Core.Models;
    using Microsoft.Extensions.Configuration;

    public class ReportBuilder
    {
        private readonly IFunctionSettings functionSettings;
        private readonly IConfiguration configuration;

        public ReportBuilder(IFunctionSettings functionSettings, IConfiguration configuration)
        {
            this.functionSettings = functionSettings;
            this.configuration = configuration;
        }

        public string GetReport()
        {
            StringBuilder builder = new StringBuilder();

            BuildHeader(builder);
            BuildFunctionReport(builder);
            BuildConfigurationReport(builder);
            BuildFooter(builder);

            return builder.ToString();
        }

        private void BuildHeader(StringBuilder builder)
        {
            builder.AppendLine("*** .NET Core Kubeless Runtime - Report ***");
            builder.AppendLine("--------------------------------------------------");
            builder.AppendLine();
        }

        private void BuildFunctionReport(StringBuilder builder)
        {
            builder.AppendKeyValue("Module/Class name", this.functionSettings.ModuleName);
            builder.AppendKeyValue("Function Handler name", this.functionSettings.FunctionHandler);
            builder.AppendKeyValue("Code file path", this.functionSettings.Code.FilePath);
            builder.AppendCode("Code content", this.functionSettings.Code.Content);
            builder.AppendKeyValue("Requirements file path", this.functionSettings.Requirements.FilePath);
            builder.AppendCode("Requirements content", this.functionSettings.Requirements.Content);
            builder.AppendKeyValue("Assembly file path", this.functionSettings.Assembly.FilePath);
            builder.AppendKeyValue("Assembly exists", ((BinaryContent)this.functionSettings.Assembly).Exists.ToString());
        }

        private void BuildConfigurationReport(StringBuilder builder)
        {
            builder.AppendKeyValue("CodePath", this.configuration["Compiler:CodePath"]);
            builder.AppendKeyValue("RequirementsPath", this.configuration["Compiler:RequirementsPath"]);
            builder.AppendKeyValue("FunctionAssemblyPath", this.configuration["Compiler:FunctionAssemblyPath"]);
        }

        private void BuildFooter(StringBuilder builder)
        {
            builder.AppendLine();
            builder.AppendLine("--------------------------------------------------");
            builder.AppendLine(DateTime.Now.ToString());
        }
    }
}