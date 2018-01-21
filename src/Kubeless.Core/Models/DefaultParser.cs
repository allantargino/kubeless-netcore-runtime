namespace Kubeless.Core.Models
{
    using Kubeless.Core.Interfaces;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class DefaultParser : IParser
    {
        public SyntaxTree ParseText(string code)
        {
            return CSharpSyntaxTree.ParseText(code);
        }
    }
}
