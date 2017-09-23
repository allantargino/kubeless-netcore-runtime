namespace Kubeless.Core.Interfaces
{
    public interface IInvoker
    {
        object Execute(params object[] parameters);
    }
}
