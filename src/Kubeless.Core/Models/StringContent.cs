namespace Kubeless.Core.Models
{
    using System.IO;
    using Kubeless.Core.Interfaces;

    public class StringContent : IFileContent<string>
    {
        public StringContent(string filePath)
        {
            this.FilePath = filePath;
            if (File.Exists(filePath))
            {
                this.Content = File.ReadAllText(filePath);
            }
        }

        public string FilePath { get; }
        
        public string Content { get; }
    }
}
