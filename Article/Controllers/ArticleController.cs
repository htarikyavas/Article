using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Article.Core.Models;
using Article.Services.Articles;
using Microsoft.AspNetCore.Mvc;

namespace Article.WebApi.Controllers
{
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService articleService;

        public ArticleController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        [HttpGet("article")]
        public async Task<List<ArticleModel>> All()
        {
            return await articleService.All();
        }

        [HttpGet("article/{id}")]
        public async Task<ArticleModel> Single(int id)
        {
            return await articleService.Get(id);
        }

        [HttpDelete("article/{id}")]
        public async Task Delete(int id)
        {
            await articleService.Delete(id);
        }

        [HttpPut("article")]
        public async Task<ArticleModel> Insert([FromBody] ArticleModel article)
        {
            var id = await articleService.Add(article);
            return await articleService.Get(id);
        }

        [HttpPatch("article/{id}")]
        public async Task<ArticleModel> Update(int id, [FromBody] ArticleModel article)
        {
            await articleService.Update(id, article);
            return await articleService.Get(id);
        }
    }
}