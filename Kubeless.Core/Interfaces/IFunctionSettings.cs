namespace Kubeless.Core.Interfaces
{
    public interface IFunctionSettings
    {
        IFileContent Code { get; }
        IFileContent Requirements { get; }
        string FunctionHandler { get; }
        string ModuleName { get; }
    }
}