namespace Kubeless.Core.Models
{
    using System;
    using System.Reflection;
    
    public sealed class InvokationData 
    {
        public Assembly FunctionAssembly { get; set; }

        public Type FunctionType { get; set; }

        public object FunctionInstance { get; set; }
    }
}
