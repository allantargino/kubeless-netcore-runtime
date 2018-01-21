namespace Kubeless.WebAPI.Utils
{
    using System;
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
            string codePath = string.Concat(codePathSetting, moduleName, ".cs"); // TOOD: ModuleName = FileName? ModuleName = ClassName?
            StringContent code = new StringContent(codePath);

            string requirementsPathSetting = configuration["Compiler:RequirementsPath"];
            Guard.AgainstEmpty(requirementsPathSetting, "Compiler:RequirementsPath");
            string requirementsPath = string.Concat(requirementsPathSetting, moduleName, ".csproj");
            StringContent requirements = new StringContent(requirementsPath);

            string assemblyPathConfiguration = configuration["Compiler:FunctionAssemblyPath"];
            Guard.AgainstEmpty(assemblyPathConfiguration, "Compiler:FunctionAssemblyPath");
            string assemblyPath = string.Concat(assemblyPathConfiguration, moduleName, ".dll"); // TODO: ModuleName = DllName?
            BinaryContent assembly = new BinaryContent(assemblyPath);

            return new FunctionSettings(moduleName, functionHandler, code, requirements, assembly);
        }
    }
}
