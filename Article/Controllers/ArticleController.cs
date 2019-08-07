using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Article.Core.Models;
using Article.Services.Articles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Article.WebApi.Controllers
{
    [Authorize]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(IArticleService articleService, ILogger<ArticleController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        [HttpGet("article")]
        public async Task<List<ArticleModel>> All()
        {

            _logger.LogInformation("All Articles Listed");

            return await _articleService.All();
        }

        [HttpGet("article/{id}")]
        public async Task<ArticleModel> Single(int id)
        {
            return await _articleService.Get(id);
        }

        [HttpDelete("article/{id}")]
        public async Task Delete(int id)
        {
            await _articleService.Delete(id);
        }

        [HttpPut("article")]
        public async Task<ArticleModel> Insert([FromBody] ArticleModel article)
        {
            var id = await _articleService.Add(article);
            return await _articleService.Get(id);
        }

        [HttpPatch("article/{id}")]
        public async Task<ArticleModel> Update(int id, [FromBody] ArticleModel article)
        {
            await _articleService.Update(id, article);
            return await _articleService.Get(id);
        }
    }
}