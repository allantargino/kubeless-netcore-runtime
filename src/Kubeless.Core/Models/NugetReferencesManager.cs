using Kubeless.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Kubeless.Core.Models
{
    class NugetReferencesManager : IReferencesManager
    {
        public MetadataReference[] GetReferences()
        {
            var references = new List<MetadataReference>()
            {
                MetadataReference.CreateFromFile(typeof(HttpRequest).Assembly.Location)
            };
            return references.ToArray();
        }
    }
}
