namespace TechExpoWorld.Data.Models
{
    public class NewsArticleTag
    {
        public int NewsArticleId { get; set; }

        public NewsArticle NewsArticle { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
