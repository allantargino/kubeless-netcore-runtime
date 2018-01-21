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
            // TODO: Create (il emit) a dynamic delegate to speed up the reflection call.

            // TODO: LOAD only once
            Assembly assembly = Assembly.Load(function.FunctionSettings.Assembly.Content);

            // HACK: Just to make example "hasher" work.
            // TODO: Just load only once.
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
                System.Console.WriteLine(args.Name);

                if(args.Name.StartsWith("BCrypt.Net-Next")) 
                {
                    System.Console.WriteLine("LOADING BCRYPT");
                    DirectoryInfo dir = new DirectoryInfo(this.requirementsPath);
                    string dll = Path.Combine(dir.FullName, "packages/bcrypt.net-next/2.1.2/lib/netstandard2.0/BCrypt.Net-Next.dll");
                    return Assembly.LoadFile(dll);
                }

                return null;
            };

            Type type = assembly.GetType(function.FunctionSettings.ModuleName);
            object instance = Activator.CreateInstance(type);

            object returnedValue = type.InvokeMember(function.FunctionSettings.FunctionHandler,
                                    BindingFlags.Default | BindingFlags.InvokeMethod,
                                    null,
                                    instance,
                                    parameters);

            return returnedValue;
        }
    }
}
