using Article.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Article.Services.Articles
{
    public interface IArticleService
    {
        Task<ArticleModel> Get(int id);

        Task Delete(int id);

        Task<List<ArticleModel>> All();

        Task Update(int id, ArticleModel article);

        Task<int> Add(ArticleModel article);
    }
}
