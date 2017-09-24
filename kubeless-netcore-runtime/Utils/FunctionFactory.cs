using Kubeless.Core.Interfaces;
using Kubeless.Core.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace Kubeless.WebAPI.Utils
{
    public class FunctionFactory
    {
        public static IFunctionSettings BuildFunctionSettings(IConfiguration configuration)
        {
            var moduleName = Environment.GetEnvironmentVariable("MOD_NAME");
            if (string.IsNullOrEmpty(moduleName))
                throw new ArgumentNullException("MOD_NAME");

            var functionHandler = Environment.GetEnvironmentVariable("FUNC_HANDLER");
            if (string.IsNullOrEmpty(moduleName))
                throw new ArgumentNullException("FUNC_HANDLER");

            //TODO: Move paths to appsettings.json
            //var codePath = string.Concat("/kubeless/", className, ".cs"); //Linux fixed path
            var codePath = string.Concat(@"..\examples\", moduleName, ".cs"); //Windows relative tests path
            var code = new StringContent(codePath);

            //var requirementsPath = string.Concat("/kubeless/", "requirements", ".xml"); //Linux fixed path
            var requirementsPath = string.Concat(@"..\examples\", "requirements", ".xml"); //Windows relative tests path
            var requirements = new StringContent(requirementsPath);

            var assemblyPath = string.Concat(configuration["Compiler:FunctionAssemblyPath"], moduleName, ".dll");
            var assembly = new BinaryContent(assemblyPath);

            return new FunctionSettings(moduleName, functionHandler, code, requirements, assembly);
        }

        public static IFunction BuildFunction(IConfiguration configuration)
        {
            var settings = BuildFunctionSettings(configuration);
            return new Function(settings);
        }
    }
}
