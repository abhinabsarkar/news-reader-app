using System;
using System.Configuration;
//Third party api for article extraction - if you are not behind proxy
//using Aylien.TextApi;
using System.Net.Http;
using System.Net.Http.Headers;
using ArticleReader.Models;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ArticleReader.TextToSpeech
{
    public class ArticleExtraction
    {
        /// <summary>
        /// Extracts the article from the url using Text analysis API
        /// You can use third party text analysis API like AYLIEN or create your own.
        /// </summary>
        /// <param name="url">The url of the article you want to listen to</param>
        /// <returns></returns>
        public String ExtractArticle(string url)
        {
            try
            {
                string textAnalysisAppId = ConfigurationManager.AppSettings["TextAnalysisAppId"];
                string textAnalysisKey = ConfigurationManager.AppSettings["TextAnalysisKey"];
                string textAnalysisEndpoint = ConfigurationManager.AppSettings["TextAnalysisEndPoint"];
                string articleExtract = null;
                Proxy proxy = new Proxy();
                HttpClientHandler handler = proxy.ProxyHandler();
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("X-AYLIEN-TextAPI-Application-ID", textAnalysisAppId);
                httpClient.DefaultRequestHeaders.Add("X-AYLIEN-TextAPI-Application-Key", textAnalysisKey);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Add parameter url
                string parameter = "?url=" + url;
                HttpResponseMessage response = httpClient.GetAsync(textAnalysisEndpoint + parameter).Result;

                if (response.IsSuccessStatusCode)
                {
                    ArticleModel article = new ArticleModel();
                    JsonConvert.PopulateObject(response.Content.ReadAsStringAsync().Result, article);                    
                    articleExtract = article.text;
                }
                return articleExtract;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}