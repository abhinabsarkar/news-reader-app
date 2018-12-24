using System;
using System.Configuration;
using System.Net.Http;

namespace ArticleReader.TextToSpeech
{
    public class Authentication
    {
        /// <summary>
        /// Fetch auth token from the Azure
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            try
            {
                string accessToken = "";
                string tokenUri = ConfigurationManager.AppSettings["tokenuri"];
                string textToSpeechAPIKey = ConfigurationManager.AppSettings["texttospeechapikey"];
                Proxy proxy = new Proxy();
                HttpClientHandler handler = proxy.ProxyHandler();
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", textToSpeechAPIKey);
                HttpResponseMessage response = httpClient.PostAsync(tokenUri, new StringContent("application/jwt")).Result;
                if (response.IsSuccessStatusCode)
                {
                    accessToken = response.Content.ReadAsStringAsync().Result;
                }
                return accessToken;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}