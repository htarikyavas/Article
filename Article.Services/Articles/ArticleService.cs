using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Article.Core.Models;
using Article.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Article.Services.Articles
{
    public class ArticleService : IArticleService
    {

        private readonly IRepository<Article.Core.Domain.Article> _articleRepository;

        public ArticleService(IRepository<Article.Core.Domain.Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<int> Add(ArticleModel article)
        {
            return await _articleRepository.Add(new Article.Core.Domain.Article()
            {
                Title = article.Title,
                Body = article.Body,
                AuthorId = article.AuthorId
            });
        }

        public async Task<List<ArticleModel>> All()
        {
            return await _articleRepository.All()
                .Select(p => new ArticleModel
                {
                    Id = p.Id,
                    AuthorId = p.AuthorId,
                    Body = p.Body,
                    Title = p.Title,
                    AuthorEmail = p.Author.Email,
                    AuthorName = p.Author.Name,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task Delete(int id)
        {
            await _articleRepository.Delete(await _articleRepository.Get(id));
        }

        public async Task<ArticleModel> Get(int id)
        {
            return await _articleRepository
                .Where(a => a.Id == id)
               .Select(p => new ArticleModel
               {
                   Id = p.Id,
                   AuthorId = p.AuthorId,
                   Body = p.Body,
                   Title = p.Title,
                   AuthorEmail = p.Author.Email,
                   AuthorName = p.Author.Name,
                   CreatedAt = p.CreatedAt,
                   UpdatedAt = p.UpdatedAt
               })
                .FirstOrDefaultAsync();
        }

        public async Task Update(int id, ArticleModel article)
        {
            var articleEntity = await _articleRepository.Get(id);

            articleEntity.Title = article.Title;
            articleEntity.Body = article.Body;
            articleEntity.AuthorId = article.AuthorId;

            await _articleRepository.Update(articleEntity);
        }
    }
}
