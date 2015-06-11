using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace WSEzer
{
    public class ClientMessageInspector : IClientMessageInspector
    {
        private readonly IConfiguration _configuration;

        public Action<string> OnBeforeSendRequest { get; set; }
        public Action<string> OnAfterReceiveReply { get; set; }

        public ClientMessageInspector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            request.Headers.Add(new SoapSecurityHeader(_configuration.Username, _configuration.Password));

            if (OnBeforeSendRequest != null)
            {
                OnBeforeSendRequest.Invoke(request.ToString());
            }

            return Guid.NewGuid();
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (OnAfterReceiveReply != null)
            {
                OnAfterReceiveReply.Invoke(reply.ToString());
            }
        }
    }
}