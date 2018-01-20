namespace Kubeless.Core.Models
{
    using Kubeless.Core.Interfaces;
    using Microsoft.CodeAnalysis;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class SharedReferencesManager : IReferencesManager
    {
        private static readonly string DotNetCoreSharedRefVersion = Environment.GetEnvironmentVariable("DOTNETCORESHAREDREF_VERSION");

        private static string SharedPath 
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

                return Path.Combine(prefix, "dotnet", "shared", "Microsoft.NETCore.App", DotNetCoreSharedRefVersion);
            }
        }

        public MetadataReference[] GetReferences()
        {
            IEnumerable<string> dlls = Directory.EnumerateFiles(SharedPath, "*.dll");

            IList<MetadataReference> references = new List<MetadataReference>()
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            };

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
