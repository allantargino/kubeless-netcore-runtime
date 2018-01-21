namespace Kubeless.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Kubeless.Core.Interfaces;
    using Microsoft.CodeAnalysis;

    public class PackageReferencesManager : IReferencesManager
    {
        private readonly IFunctionSettings functionSettings;

        public PackageReferencesManager(IFunctionSettings functionSettings)
        {
            this.functionSettings = functionSettings;
        }

        public MetadataReference[] GetReferences()
        {
            IList<MetadataReference> references = new List<MetadataReference>();

            foreach(string dll in ReferenceAssemblyFinder.FindReferenceAssembies(this.functionSettings))
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
