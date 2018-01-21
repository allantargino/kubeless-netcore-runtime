namespace Kubeless.WebAPI.Utils
{
    using System;
    using System.IO;
    using Kubeless.Core.Interfaces;
    using Kubeless.Core.Models;
    using Microsoft.Extensions.Configuration;

    public static class FunctionFactory
    {
        public static IFunction BuildFunction(IConfiguration configuration)
        {
            IFunctionSettings settings = BuildFunctionSettings(configuration);
            return new Function(settings);
        }

        private static IFunctionSettings BuildFunctionSettings(IConfiguration configuration)
        {
            string moduleName = Environment.GetEnvironmentVariable("MOD_NAME");
            Guard.AgainstEmpty(moduleName, "MOD_NAME");

            string functionHandler = Environment.GetEnvironmentVariable("FUNC_HANDLER");
            Guard.AgainstEmpty(functionHandler, "FUNC_HANDLER");

            string codePathSetting = configuration["Compiler:CodePath"];
            Guard.AgainstEmpty(codePathSetting, "Compiler:CodePath");
            string codePath = Path.Combine(codePathSetting, string.Concat(moduleName, ".cs")); 
            StringContent code = new StringContent(codePath);

            string requirementsPathSetting = configuration["Compiler:RequirementsPath"];
            Guard.AgainstEmpty(requirementsPathSetting, "Compiler:RequirementsPath");
            string requirementsPath = Path.Combine(requirementsPathSetting, string.Concat(moduleName, ".csproj"));
            StringContent project = new StringContent(requirementsPath);

            string projectAssetsPath = Path.Combine(requirementsPathSetting, "obj", "project.assets.json");
            StringContent projectAssets = new StringContent(projectAssetsPath);

            string assemblyPathConfiguration = configuration["Compiler:FunctionAssemblyPath"];
            Guard.AgainstEmpty(assemblyPathConfiguration, "Compiler:FunctionAssemblyPath");
            string assemblyPath = Path.Combine(assemblyPathConfiguration, string.Concat(moduleName, ".dll"));
            BinaryContent assembly = new BinaryContent(assemblyPath);

            return new FunctionSettings(moduleName, functionHandler, code, project, projectAssets, assembly);
        }
    }
}
