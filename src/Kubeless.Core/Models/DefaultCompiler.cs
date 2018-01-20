namespace Kubeless.Core.Models
{
    using Kubeless.Core.Exceptions;
    using Kubeless.Core.Interfaces;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Emit;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class DefaultCompiler : ICompiler
    {
        public DefaultCompiler(IParser parser, IReferencesManager referenceManager)
        {
            this.Parser = parser;
            this.ReferencesManager = referenceManager;
        }

        public IParser Parser { get; }
        
        public IReferencesManager ReferencesManager { get; }

        private CSharpCompilation GetCompilationEngine(string assemblyName, string code)
        {
            SyntaxTree syntaxTree = this.Parser.ParseText(code);
            MetadataReference[] references = this.ReferencesManager.GetReferences();
            return CreateCompilation(assemblyName, syntaxTree, references);
        }
        
        private CSharpCompilation CreateCompilation(string assemblyName, SyntaxTree syntaxTree, MetadataReference[] references)
        {
            return CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }
        
        public void Compile(IFunction function)
        {
            string assemblyName = string.Concat(function.FunctionSettings.ModuleName, ".dll");
            string code = function.FunctionSettings.Code.Content;

            CSharpCompilation compilation = GetCompilationEngine(assemblyName, code);
            using (MemoryStream stream = new MemoryStream())
            {
                EmitResult result = compilation.Emit(stream);
                if (!result.Success)
                {
                    throw new CompilationException(GetCriticalErrors(result));
                }

                stream.Seek(0, SeekOrigin.Begin);
                ((BinaryContent)function.FunctionSettings.Assembly).UpdateBinaryContent(stream.ToArray());
            }
        }

        private IEnumerable<Diagnostic> GetCriticalErrors(EmitResult result)
        {
            return result.Diagnostics.Where(diagnostic =>
                                    diagnostic.IsWarningAsError ||
                                    diagnostic.Severity == DiagnosticSeverity.Error);
        }
    }
}