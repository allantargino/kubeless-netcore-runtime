namespace Kubeless.Core.Models
{
    using Kubeless.Core.Interfaces;
    using Microsoft.CodeAnalysis;
    using System.Collections.Generic;
    using System;
    using System.Runtime.InteropServices;
    using System.IO;
    using System.Reflection;
    using System.Linq;
    using Kubeless.Core.Filters;

    public class StoreReferencesManager : IReferencesManager
    {
        private static string StorePath 
        {
            get 
            {
                string prefix = string.Empty;

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    prefix = Environment.GetEnvironmentVariable("ProgramFiles");
                }
                else
                {
                    prefix = Path.Combine("/", "usr", "share");

                    // Maybe the dotnet folder is located in /usr/local/shared (in development on an mac f.e.).
                    if(!Directory.Exists(Path.Combine(prefix, "dotnet")))
                    {
                        prefix = Path.Combine("/", "usr", "local", "share");
                    }
                }

                return Path.Combine(prefix, "dotnet", "store", "x64", "netcoreapp2.0");
            }
        }

        public MetadataReference[] GetReferences()
        {
            IEnumerable<string> dlls = Directory
                .EnumerateFiles(StorePath, "*.dll", SearchOption.AllDirectories)
                .ApplyFilterOnDllVersion();

            IEnumerable<FileInfo> dllFiles = from d in dlls select new FileInfo(d);

            IList<MetadataReference> references = new List<MetadataReference>();

            // Not every .dll on the directory can be used during compilation. Some of them are just metadata.
            // The following try-catch statement ensures only usable assemblies will be added to compilation process.
            foreach (string dll in dlls)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(dll);
                    references.Add(MetadataReference.CreateFromFile(dll));
                }
                catch (BadImageFormatException) {}
                catch
                {
                    throw;
                }
            }

            return references.ToArray();
        }
    }
}
