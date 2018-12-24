namespace ArticleReader.Models
{
    public class ArticleModel
    {
        public string text { get; set; }
        public string language { get; set; }
        public object[] categories { get; set; }

    }
}