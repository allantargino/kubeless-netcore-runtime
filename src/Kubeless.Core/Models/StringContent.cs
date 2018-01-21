namespace Kubeless.Core.Models
{
    using System.IO;
    using Kubeless.Core.Interfaces;

    public class StringContent : IFileContent<string>
    {
        public StringContent(string filePath)
        {
            this.FilePath = filePath;
            if (this.FileExists)
            {
                this.Content = File.ReadAllText(filePath);
            }
        }

        public string FilePath { get; }

        public FileInfo FileInfo => new FileInfo(this.FilePath);
        
        public bool FileExists => File.Exists(this.FilePath);

        public string Content { get; }
    }
}
