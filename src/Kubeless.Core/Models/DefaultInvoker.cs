namespace Kubeless.Core.Models
{
    using Kubeless.Core.Interfaces;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.IO;

    public class DefaultInvoker : IInvoker
    {
        private readonly string requirementsPath;

        private InvokationData invokationData;

        public DefaultInvoker(string requirementsPath)
        {
            this.requirementsPath = requirementsPath;
        }

        public async Task<object> Execute(IFunction function, params object[] parameters)
        {
            object result = this.ExecuteFunction(function, parameters);
            if(result is Task) 
            {
                return await (dynamic)result; // NOTE: Is there a better way for doing this?
            }
            else 
            {
                return result;
            }
        }

        private object ExecuteFunction(IFunction function, params object[] parameters)
        {
            // Create invokation data if none exists. This caches assembly and type loading.
            if(this.invokationData == null) 
            {
                this.invokationData = this.CreateInvokationData(function);
            }

            // TODO: Create (il emit) a dynamic delegate to speed up the reflection call.
            object returnedValue = this.invokationData.FunctionType.InvokeMember(
                function.FunctionSettings.FunctionHandler,
                BindingFlags.Default | BindingFlags.InvokeMethod,
                null,
                this.invokationData.FunctionInstance,
                parameters);

            return returnedValue;
        }

        private InvokationData CreateInvokationData(IFunction function)
        {
            Assembly assembly = Assembly.Load(function.FunctionSettings.Assembly.Content);

            // HACK: Just to make example "hasher" work.
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {

                AssemblyName referenceAssembly = args.RequestingAssembly.GetReferencedAssemblies().FirstOrDefault(x => x.FullName == args.Name);
                System.Console.WriteLine(referenceAssembly.FullName);

                if(args.Name.StartsWith("BCrypt.Net-Next")) 
                {
                    DirectoryInfo dir = new DirectoryInfo(this.requirementsPath);
                    string dll = Path.Combine(dir.FullName, "packages/bcrypt.net-next/2.1.2/lib/netstandard2.0/BCrypt.Net-Next.dll");
                    return Assembly.LoadFile(dll);
                }

                return null;
            };

            Type type = assembly.GetExportedTypes().FirstOrDefault(x => x.Name == function.FunctionSettings.ModuleName);
            object instance = Activator.CreateInstance(type);

            return new InvokationData 
            {
                FunctionAssembly = assembly,
                FunctionType = type,
                FunctionInstance = instance,
            };
        }
    }
}
