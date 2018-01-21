namespace Kubeless.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Kubeless.Core.Interfaces;
    using Microsoft.CodeAnalysis;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class PackageReferencesManager : IReferencesManager
    {
        private readonly IFunctionSettings functionSettings;

        public PackageReferencesManager(IFunctionSettings functionSettings)
        {
            this.functionSettings = functionSettings;
        }

        public MetadataReference[] GetReferences()
        {
            // TODO: We should use the target framework info from the csproj file to map to the correct
            //       path informations inside the project.assets.file.
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

            IList<MetadataReference> references = new List<MetadataReference>();

            // Read package references from obj/project.assets.json if it exists.
            if(this.functionSettings.Project.FileExists && this.functionSettings.ProjectAssets.FileExists)
            {
                JObject projectAssets = JsonConvert.DeserializeObject<JObject>(this.functionSettings.ProjectAssets.Content);
                foreach(JProperty dependency in projectAssets["targets"][".NETStandard,Version=v2.0"])
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
                    string projectPath = this.functionSettings.Project.FileInfo.DirectoryName;

                    string dll = Path.Combine(projectPath, "packages", packagePath, libaryPath);

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
            }

            return references.ToArray();
        }
    }
}
