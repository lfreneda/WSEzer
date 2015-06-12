using System.ServiceModel;
using WSEzer.Configuration;

namespace WSEzer.Extensions
{
    public static class ClientBaseExtensions
    {
        public static void AddSecurity<TChannel>(this ClientBase<TChannel> clientBase, IConfiguration configuration)
            where TChannel : class
        {
            clientBase.Endpoint.Behaviors.Add(new WseSecurityEndpointBehavior(new ClientMessageInspector(configuration)));
        }
    }
}