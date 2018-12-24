using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ArticleReader.ErrorHandler;
using ArticleReader.Models;
using ArticleReader.TextToSpeech;
using Microsoft.ApplicationInsights;

namespace ArticleReader.Controllers
{
    public class HomeController : Controller
    {
        TelemetryClient tc = new TelemetryClient();
        public ActionResult Index()
        {            
            SpeechModel speechModel = new SpeechModel();
            return View(speechModel);
        }

        [HttpPost]
        public ActionResult Index(SpeechModel speechModel, string ArticleUrl)
        {
            try
            {                
                string accessToken;
                //Extract contents from an article
                tc.TrackTrace("Extract contents from article");
                ArticleExtraction articleExtraction = new ArticleExtraction();
                StringBuilder articleExtract = new StringBuilder();
                articleExtract.Append(articleExtraction.ExtractArticle(ArticleUrl));
                //Add end of article note
                articleExtract.AppendLine("This article is over. Thanks for listening.");

                //ErrorLogger.Debug("Article extract: " + articleExtract);
                //ErrorLogger.Debug("Get access token");

                tc.TrackTrace("Article extract: " + articleExtract);
                tc.TrackTrace("Get access token");
                //Get access token for cognitive speech API
                Authentication auth = new Authentication();
                accessToken = auth.GetAccessToken();

                ViewBag.Content = articleExtract.ToString();                
                ViewBag.Token = accessToken;
                ViewBag.Key = speechModel.SubscriptionKey;
                ViewBag.LocaleCode = speechModel.LocaleCode;

                //return View("ReadArticle", speechModel);
                return View(speechModel);
            }
            catch (Exception ex)
            {
                //return RedirectToAction("Index");  
                //ErrorLogger.Debug(ex.Message);
                //ErrorLogger.Debug(ex.StackTrace);
                //ErrorLogger.Debug(ex.InnerException.ToString());
                tc.TrackException(ex);
                return View("Error");
            }
        }

        /// <summary>
        /// Plays the article content by invoking the speech API
        /// </summary>
        /// <param name="token">Access token for calling the speech API</param>
        /// <param name="key">Speech API key</param>
        /// <param name="content">Article content which has to be synthesized</param>
        /// <param name="locale">Language variant</param>
        /// <returns></returns>
        public async Task<ActionResult> PlayAudio(string token, string key, string content, string locale)
        {
            try
            {
                if (!String.IsNullOrEmpty(content))
                {
                    Reader reader = new Reader();
                    var waveBytes = reader.ParseAndPlay(token, key, content, locale);

                    return File(await Task<string>.Run(() => waveBytes), "audio/mpeg", "audio.mp3");
                }
                return File("", "audio/mpeg");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ReadArticle()
        {
            return View();
        }
    }
}