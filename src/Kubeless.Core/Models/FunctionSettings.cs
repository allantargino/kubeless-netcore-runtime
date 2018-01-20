namespace Kubeless.Core.Models
{
    using Kubeless.Core.Interfaces;
    
    public sealed class FunctionSettings : IFunctionSettings
    {
        public FunctionSettings(string moduleName, string functionHandler, IFileContent<string> code, IFileContent<string> requirements, IFileContent<byte[]> assembly)
        {
            this.ModuleName = moduleName;
            this.FunctionHandler = functionHandler;
            this.Code = code;
            this.Requirements = requirements;
            this.Assembly = assembly;
        }

        public string ModuleName { get; private set; }

        public string FunctionHandler { get; private set; }

        public IFileContent<string> Code { get; private set; }

        public IFileContent<string> Requirements { get; private set; }
        
        public IFileContent<byte[]> Assembly { get; private set; }
    }
}
