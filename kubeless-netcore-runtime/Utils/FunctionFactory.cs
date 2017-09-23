using Kubeless.Core.Interfaces;
using Kubeless.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kubeless.WebAPI.Utils
{
    public class FunctionFactory
    {
        public static IFunctionSettings BuildKubelessFunction()
        {
            //TODO: remove
            Environment.SetEnvironmentVariable("MOD_NAME", "mycode");
            Environment.SetEnvironmentVariable("FUNC_HANDLER", "execute");

            var moduleName = Environment.GetEnvironmentVariable("MOD_NAME");
            if (string.IsNullOrEmpty(moduleName))
                throw new ArgumentNullException("MOD_NAME");

            var functionHandler = Environment.GetEnvironmentVariable("FUNC_HANDLER");
            if (string.IsNullOrEmpty(moduleName))
                throw new ArgumentNullException("FUNC_HANDLER");

            //TODO: mount file system on linux and document
            //var codePath = string.Concat("/kubeless/", className, ".cs"); //Linux fixed path
            var codePath = string.Concat(@"..\examples\", moduleName, ".cs"); //Windows relative tests path
            var code = new FileContent(codePath);

            //var dependenciesPath = string.Concat("/kubeless/", "dependencies", ".xml"); //Linux fixed path
            var dependenciesPath = string.Concat(@"..\examples\", "dependencies", ".xml"); //Windows relative tests path
            var requirements = new FileContent(dependenciesPath);

            return new FunctionSettings(moduleName, functionHandler, code, requirements);
        }
    }
}
