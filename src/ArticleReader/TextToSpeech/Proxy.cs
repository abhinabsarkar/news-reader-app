using System.Configuration;
using System.Net;
using System.Net.Http;

namespace ArticleReader.TextToSpeech
{
    public class Proxy
    {
        public HttpClientHandler ProxyHandler()
        {
            string proxyusername = ConfigurationManager.AppSettings["proxyusername"];
            string proxypassword = ConfigurationManager.AppSettings["proxypassword"];
            var defaultProxy = WebRequest.DefaultWebProxy;
            defaultProxy.Credentials = new NetworkCredential(proxyusername, proxypassword);
            var handler = new HttpClientHandler { Proxy = defaultProxy };
            return handler;
        }
    }
}