using System.Collections.Generic;
using System.Threading.Tasks;
using Article.Core.Domain;
using Article.Services.Authors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Article.WebApi.Controllers
{
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IAuthorService authorService, ILogger<AuthorController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }

        [HttpGet("author")]
        public async Task<List<AuthorModel>> All()
        {
            _logger.LogInformation("All Authors Listed");

            return await _authorService.All();
        }

        [HttpGet("author/{id}")]
        public async Task<AuthorModel> Single(int id)
        {
            return await _authorService.Get(id);
        }

        [HttpDelete("author/{id}")]
        public async Task Delete(int id)
        {
            await _authorService.Delete(id);
        }

        [HttpPut("author")]
        public async Task<AuthorModel> Insert([FromBody] AuthorModel author)
        {
            var id = await _authorService.Add(author);
            return await _authorService.Get(id);
        }

        [HttpPatch("author/{id}")]
        public async Task<AuthorModel> Update(int id, [FromBody] AuthorModel author)
        {
            await _authorService.Update(id, author);
            return await _authorService.Get(id);
        }
    }
}