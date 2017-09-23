using Kubeless.Core.Interfaces;
using System.IO;

namespace Kubeless.Core.Models
{
    public class FileContent : IFileContent
    {
        public string FilePath { get; }
        public string Content { get; }

        public FileContent(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(nameof(filePath));

            this.FilePath = filePath;
            this.Content = File.ReadAllText(filePath);
        }
    }
}
