namespace Kubeless.Core.Models
{
    using Kubeless.Core.Interfaces;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    public class DefaultReferencesManager : IReferencesManager
    {
        private readonly IEnumerable<IReferencesManager> referencesManager;

        public DefaultReferencesManager(IFunctionSettings functionSettings)
        {
            this.referencesManager = new List<IReferencesManager>()
            {
                new SharedReferencesManager(),
                new StoreReferencesManager(),
                new PackageReferencesManager(functionSettings),
            };
        }

        public MetadataReference[] GetReferences()
        {
            List<MetadataReference> references = new List<MetadataReference>();
            foreach (IReferencesManager manager in referencesManager)
            {
                references.AddRange(manager.GetReferences());
            }
            return references.ToArray();
        }
    }
}
