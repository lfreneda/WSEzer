using System;
using System.ServiceModel.Channels;
using System.Xml;

namespace WSEzer
{
    public class SoapSecurityHeader : MessageHeader
    {
        private readonly string _password, _username;

        public SoapSecurityHeader(string username, string password)
        {
            _password = password;
            _username = username;
        }

        public override string Name
        {
            get { return "Security"; }
        }

        public override string Namespace
        {
            get { return "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"; }
        }

        protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement("wsse", Name, Namespace);
            writer.WriteAttributeString("xmlns:wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            writer.WriteAttributeString("s:mustUnderstand", "1");
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            string nonce = GenerateNonce();
            string createAt = CreatedAt();
            byte[] password = (nonce + createAt + _password).ToSha1();

            writer.WriteRaw(string.Format(@"
              <wsse:UsernameToken wsu:Id=""UsernameToken-{0}"">
                <wsse:Username>{1}</wsse:Username>
                <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest"">{2}</wsse:Password>
                <wsse:Nonce EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">{3}</wsse:Nonce>
                <wsu:Created>{4}</wsu:Created>
              </wsse:UsernameToken>", GenerateUsernameToken(), _username, password.ToBase64String(), nonce.ToBase64String(), createAt)
            );
        }

        private static string GenerateNonce()
        {
            return Guid.NewGuid().ToString();
        }

        private static string CreatedAt()
        {
            DateTime created = DateTime.Now.ToUniversalTime();
            return created.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        private static string GenerateUsernameToken()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }
    }
}