namespace Kubeless.Core.Interfaces
{
    using Microsoft.CodeAnalysis;
    
    public interface IReferencesManager
    {
        MetadataReference[] GetReferences();
    }
}
