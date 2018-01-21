namespace Kubeless.Core.Models
{
    using System;
    using System.IO;
    using System.Reflection;
    using Kubeless.Core.Interfaces;
    using Microsoft.CodeAnalysis;

    public class PackageReferencesManager : IReferencesManager
    {
        private readonly string requirementsPath;

        public PackageReferencesManager(string requirementsPath)
        {
            this.requirementsPath = requirementsPath;
        }

        public MetadataReference[] GetReferences()
        {
            // TODO: We must read the referenced assemblies and create MetadataReferences from them.
            // HACK: This is just for making the example "hasher" work.
            DirectoryInfo dir = new DirectoryInfo(this.requirementsPath);
            string dll = Path.Combine(dir.FullName, "packages/bcrypt.net-next/2.1.2/lib/netstandard2.0/BCrypt.Net-Next.dll");
            
            Assembly assembly = Assembly.LoadFile(dll);

            return new MetadataReference[] 
            {
                MetadataReference.CreateFromFile(dll)
            };
        }
    }
}
