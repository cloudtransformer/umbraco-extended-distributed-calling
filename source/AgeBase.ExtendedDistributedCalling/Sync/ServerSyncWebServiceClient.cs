using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using Umbraco.Core.IO;

namespace AgeBase.ExtendedDistributedCalling.Sync
{
    [WebServiceBinding(
        Name = "CacheRefresherSoap",
        Namespace = "http://umbraco.org/webservices/")]
    internal class ServerSyncWebServiceClient : SoapHttpClientProtocol
    {
        public ServerSyncWebServiceClient(string domain)
        {
            Url = "http://" + domain.Trim() + IOHelper.ResolveUrl(SystemDirectories.WebServices) + "/cacheRefresher.asmx";
        }

        [SoapDocumentMethod("http://umbraco.org/webservices/RefreshAll",
            RequestNamespace = "http://umbraco.org/webservices/",
            ResponseNamespace = "http://umbraco.org/webservices/",
            Use = SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public void RefreshAll(Guid uniqueIdentifier, string Login, string Password)
        {
            Invoke("RefreshAll", new object[] { uniqueIdentifier, Login, Password });
        }
    }
}