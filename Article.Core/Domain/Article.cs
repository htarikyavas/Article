namespace Article.Core.Domain
{
    public class Article : BaseEntity
    {

        public int? AuthorId { get; set; }

        public virtual Author Author { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

    }
}
