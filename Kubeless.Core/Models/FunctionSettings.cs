using Kubeless.Core.Interfaces;

namespace Kubeless.Core.Models
{
    public sealed class FunctionSettings : IFunctionSettings
    {
        public string ModuleName { get; private set; }
        public string FunctionHandler { get; private set; }
        public IFileContent Code { get; private set; }
        public IFileContent Requirements { get; private set; }

        public FunctionSettings(string moduleName, string functionHandler, IFileContent code, IFileContent requirements)
        {
            ModuleName = moduleName;
            FunctionHandler = functionHandler;
            Code = code;
            Requirements = requirements;
        }

    }
}
