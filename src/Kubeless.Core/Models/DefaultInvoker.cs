namespace Kubeless.Core.Models
{
    using Kubeless.Core.Interfaces;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    public class DefaultInvoker : IInvoker
    {
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
            Assembly assembly = Assembly.Load(function.FunctionSettings.Assembly.Content);
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
