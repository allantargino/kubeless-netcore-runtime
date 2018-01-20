namespace Kubeless.Core.Models
{
    using Kubeless.Core.Interfaces;
    
    public class Function : IFunction
    {
        public Function(IFunctionSettings functionSettings)
        {
            this.FunctionSettings = functionSettings;
        }

        public IFunctionSettings FunctionSettings { get; }

        public bool IsCompiled()
        {
            return ((BinaryContent)FunctionSettings.Assembly).Exists;
        }
    }
}
