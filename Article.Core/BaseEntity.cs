using System;
using System.ComponentModel.DataAnnotations;

namespace Article.Core
{
    public class BaseEntity
    {

        [Key]
        public int Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
