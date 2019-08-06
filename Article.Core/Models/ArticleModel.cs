namespace Article.Core.Models
{
    public class ArticleModel
    {
        public int? AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
