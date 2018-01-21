namespace Kubeless.Core.Models
{
    using System;
    using System.Reflection;
    
    public sealed class InvocationData 
    {
        public Assembly FunctionAssembly { get; set; }

        public Type FunctionType { get; set; }

        public object FunctionInstance { get; set; }

        public MethodInvoker MethodInvoker { get; set; }
    }
}
