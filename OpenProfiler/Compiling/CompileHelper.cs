using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Runtime.Loader;

namespace OpenProfiler.Compiling
{
    public static class CompileHelper
    {
        public static Assembly CompileAssembly(
            string assemblyName,
            List<string> sourceTexts,
            List<string> dependencyPaths)
        {
            var syntaxTrees = sourceTexts.Select(x => CSharpSyntaxTree.ParseText(x));

            string assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
            var references = new List<MetadataReference>()
            {
                MetadataReference.CreateFromFile(typeof(OpenProfilerInfrastructure).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "netstandard.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll"))
            };
            references.AddRange(dependencyPaths.Select(x => MetadataReference.CreateFromFile(x)));
            var compilation = CSharpCompilation.Create(assemblyName)
                .AddSyntaxTrees(syntaxTrees)
                .AddReferences(references)
                .WithOptions(
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var memoryStream = new MemoryStream())
            {
                var emitResult = compilation.Emit(memoryStream);
                memoryStream.Position = 0;
                if (emitResult.Success)
                {
                    var assemblyLoadContext = AssemblyLoadContext.Default;
                    return assemblyLoadContext.LoadFromStream(memoryStream);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

        }
    }
    
}
