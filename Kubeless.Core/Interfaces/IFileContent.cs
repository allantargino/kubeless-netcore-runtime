namespace Kubeless.Core.Interfaces
{
    public interface IFileContent
    {
        string Content { get; }
        string FilePath { get; }
    }
}