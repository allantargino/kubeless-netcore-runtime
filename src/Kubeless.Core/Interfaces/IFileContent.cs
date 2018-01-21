namespace Kubeless.Core.Interfaces
{
    using System.IO;

    public interface IFileContent<T>
    {
        string FilePath { get; }

        FileInfo FileInfo { get; }
        
        bool FileExists { get; }

        T Content{ get; }
    }
}