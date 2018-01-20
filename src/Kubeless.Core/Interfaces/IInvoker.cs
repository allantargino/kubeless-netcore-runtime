namespace Kubeless.Core.Interfaces
{
    using System.Threading.Tasks;

    public interface IInvoker
    {
        Task<object> Execute(IFunction function, params object[] parameters);
    }
}
