namespace Kubeless.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Kubeless.Core.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class ReferenceAssemblyFinder
    {
        public static IEnumerable<string> FindReferenceAssembies(IFunctionSettings functionSettings)
        {
            // We use the target framework info from the csproj file to map to the correct
            // path informations inside the project.assets.file.
            /*
                <PropertyGroup>
                    <TargetFramework>netstandard2.0</TargetFramework>
                </PropertyGroup>
            */
            /*
                <ItemGroup>
                    <PackageReference Include="Microsoft.AspNetCore.Http.Abstractions" Version="2.0.1" />
                    <PackageReference Include="BCrypt.Net-Next" Version="2.1.2" />
                </ItemGroup>
            */

            // Read the target fromework from the csproj file.
            if(functionSettings.Project.FileExists)
            {
                XElement project = XElement.Load(functionSettings.Project.FilePath);
                XElement targetFramework = project.XPathSelectElement("./PropertyGroup/TargetFramework");         
                string target = ReferenceAssemblyFinder.GetTargetFrameworkName(targetFramework.Value);

                // Read package references from obj/project.assets.json if it exists.
                if(functionSettings.ProjectAssets.FileExists)
                {
                    JObject projectAssets = JsonConvert.DeserializeObject<JObject>(functionSettings.ProjectAssets.Content);
                    foreach(JProperty dependency in projectAssets["targets"][target])
                    {
                        // Ignore Microsoft and System packages as they are alreay loaded.
                        if(dependency.Name.StartsWith("Microsoft") 
                            || dependency.Name.StartsWith("System") 
                            || dependency.Name.StartsWith("NETStandard.Library"))
                        {
                            continue;
                        }

                        // Get path to the desired assembly file.
                        JObject dependencyValue = (JObject)dependency.Value;
                        JProperty compile = (JProperty)dependencyValue["compile"].First;

                        string packagePath = dependency.Name; // f.e bcrypt.net-next/2.1.2
                        string libaryPath = compile.Name; // f.e. lib/netstandard2.0/BCrypt.Net-Next.dll"
                        string projectPath = functionSettings.Project.FileInfo.DirectoryName;
                        string dll = Path.Combine(projectPath, "packages", packagePath, libaryPath);    

                        yield return dll;
                    }
                }
            }
        }

        internal static string FindReferenceAssembly(AssemblyName assemblyName, IFunctionSettings functionSettings)
        {
            // NOTE: I am not sure if the assemblyName.Name is the name of the dll file.
            //       We should monitor this.
            string assembylFileName = $"{assemblyName.Name.ToLowerInvariant()}.dll";

            foreach(string dll in FindReferenceAssembies(functionSettings))
            {
                string lower = dll.ToLowerInvariant();
                if(lower.EndsWith(assembylFileName))
                {
                    return dll;
                }
                else
                {
                    continue;
                }
            }

            throw new InvalidOperationException($"The requested reference assembly was not found: {assemblyName.Name}.dll");
        }

        private static string GetTargetFrameworkName(string targetFramework)
        {
            // TODO: Use an complete official mapping list.
            switch(targetFramework)
            {
                case "netstandard2.0":
                    return ".NETStandard,Version=v2.0";
                default:
                    throw new InvalidOperationException($"The target framework is not supported: {targetFramework}");
            }
        }
    }
}
