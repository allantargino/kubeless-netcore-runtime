namespace Kubeless.Core.Models
{
    using Kubeless.Core.Interfaces;
    using System.IO;

    public class BinaryContent : IFileContent<byte[]>
    {
        public BinaryContent(string filePath)
        {
            this.FilePath = filePath;
            if (File.Exists(filePath))
            {
                this.Content = File.ReadAllBytes(filePath);
                this.Exists = true;
            }
            else
            {
                this.Exists = false;
            }
        }

        public string FilePath { get; }

        public byte[] Content { get; private set; }

        public bool Exists { get; private set; }

        public void UpdateBinaryContent(byte[] content)
        {
            this.Content = content;
            File.WriteAllBytes(this.FilePath, content);
            this.Exists = true;
        }
    }
}
