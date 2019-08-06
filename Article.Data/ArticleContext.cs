using Article.Core;
using Article.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Data
{
    public class ArticleContext : DbContext
    {

        public virtual DbSet<Article.Core.Domain.Article> Article { get; set; }

        public virtual DbSet<Author> Author { get; set; }

        public ArticleContext(DbContextOptions<ArticleContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article.Core.Domain.Article>()
                .Property(a => a.Body)
                .HasMaxLength(Int32.MaxValue);

            modelBuilder.Entity<Article.Core.Domain.Article>()
                .HasOne(a => a.Author)
                .WithMany(a => a.Articles)
                .HasForeignKey(a => a.AuthorId);
        }

        public override int SaveChanges()
        {
            AddAuitInfo();
            return base.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            AddAuitInfo();
            return base.SaveChangesAsync();
        }

        private void AddAuitInfo()
        {
            var entries = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified))
                .Select(a => (a.State == EntityState.Added, a.Entity as BaseEntity));

            foreach (var (added, entity) in entries)
            {
                if (added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
