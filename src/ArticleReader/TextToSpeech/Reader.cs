using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Media;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArticleReader.TextToSpeech
{
    public class Reader
    {
        /// <summary>
        /// Configure Speech Synthesis Markup Language (SSML), which allows you to choose 
        /// the voice and language of the response
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="gender"></param>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private string GenerateSsml(string lang, string gender, string name, string text)
        {
            var ssmlDoc = new XDocument(
                              new XElement("speak",
                                  new XAttribute("version", "1.0"),
                                  new XAttribute(XNamespace.Xml + "lang", "en-US"),
                                  new XElement("voice",
                                      new XAttribute(XNamespace.Xml + "lang", lang),
                                      new XAttribute(XNamespace.Xml + "gender", gender),
                                      new XAttribute("name", name),
                                      text)));
            return ssmlDoc.ToString();
        }

        /// <summary>
        /// Invokes the Text-To-Speech API which returns the response as an audio file (audio stream with the codec)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public async Task<byte[]> ParseAndPlay(string token, string key, string content, string locale)
        {
            try
            {
                //Request url for the speech api.
                string uri = ConfigurationManager.AppSettings["texttospeechapiuri"];
                //Generate Speech Synthesis Markup Language (SSML) 
                var requestBody = this.GenerateSsml(locale, "Female", this.ServiceName(locale), content);

                Proxy proxy = new Proxy();
                HttpClientHandler handler = proxy.ProxyHandler();

                using (var client = new HttpClient(handler))
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(uri);
                    request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    request.Headers.Add("X-Microsoft-OutputFormat", "audio-16khz-64kbitrate-mono-mp3");
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");
                    request.Content.Headers.Remove("Content-Type");
                    request.Content.Headers.Add("Content-Type", "application/ssml+xml");
                    request.Headers.Add("User-Agent", "TexttoSpeech");
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var httpStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                        Stream receiveStream = httpStream;
                        byte[] buffer = new byte[32768];

                        using (Stream stream = httpStream)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                byte[] waveBytes = null;
                                int count = 0;
                                do
                                {
                                    byte[] buf = new byte[1024];
                                    count = stream.Read(buf, 0, 1024);
                                    ms.Write(buf, 0, count);
                                } while (stream.CanRead && count > 0);

                                waveBytes = ms.ToArray();

                                return waveBytes;
                            }
                        }
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        /// <summary>
        /// Return the voice for a given locale
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        private string ServiceName(string locale)
        {
            string description = string.Empty;
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("en-US", "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)");
            values.Add("en-IN", "Microsoft Server Speech Text to Speech Voice (en-IN, Heera, Apollo)");
            values.TryGetValue(locale, out description);
            return description;
        }
    }
}