namespace Kubeless.Core.Models
{
    using Microsoft.AspNetCore.Http;

    // TODO: Custom context class to be able to remove the dependency on Microsoft.AspNetCore.Http.
    public delegate object MethodInvoker(HttpRequest request);
}