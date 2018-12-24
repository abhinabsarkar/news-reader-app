using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Web.Mvc;

namespace ArticleReader.Models
{
    public class SpeechModel
    {
        public string Content { get; set; }

        public string SubscriptionKey { get; set; } = ConfigurationManager.AppSettings["texttospeechapikey"];

        [DisplayName("Select locale & voice")]
        public string LocaleCode { get; set; } = "en-US";

        public List<SelectListItem> LocalePreference { get; set; } = new List<SelectListItem>
        {
        //new SelectListItem { Value = "NA", Text = "-Select-" },
        new SelectListItem { Value = "en-US", Text = "English (United States)"  },
        new SelectListItem { Value = "en-IN", Text = "English (India)"  }
        };

    }
}