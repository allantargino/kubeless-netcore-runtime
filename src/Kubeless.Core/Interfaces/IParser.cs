namespace Kubeless.Core.Interfaces
{
    using Microsoft.CodeAnalysis;

    public interface IParser
    {
        SyntaxTree ParseText(string code);
    }
}
