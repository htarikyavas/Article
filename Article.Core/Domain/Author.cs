using System.Collections.Generic;

namespace Article.Core.Domain
{
    public class Author : BaseEntity
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

    }
}
