using System.Collections.Generic;
using System.Threading.Tasks;
using Article.Core.Domain;
using Article.Data.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Article.Services.Authors
{
    public class AuthorService : IAuthorService
    {
        private const string _cacheKey = "Article.Cache.Key.Author";

        private readonly IRepository<Author> _authorRepository;
        private readonly IMemoryCache _cache;

        public AuthorService(IRepository<Author> authorRepository, IMemoryCache cache)
        {
            _authorRepository = authorRepository;
            _cache = cache;
        }

        public async Task<int> Add(AuthorModel author)
        {
            var addedEntityId = await _authorRepository.Add(new Author()
            {
                Name = author.Name,
                Email = author.Email
            });

            _cache.Remove(_cacheKey);

            return addedEntityId;
        }

        public async Task<List<AuthorModel>> All()
        {

            return await _cache.GetOrCreateAsync<List<AuthorModel>>(_cacheKey,
                async cacheEntry => await _authorRepository.All()
                .Select(p => new AuthorModel
                {
                    Id = p.Id,
                    Email = p.Email,
                    Name = p.Name,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync());
        }

        public async Task Delete(int id)
        {
            await _authorRepository.Delete(
               await _authorRepository.Get(id)
           );

            _cache.Remove(_cacheKey);
        }

        public async Task<AuthorModel> Get(int id)
        {
            return await _authorRepository
                .Where(a => a.Id == id)
                .Select(p => new AuthorModel
                {
                    Id = p.Id,
                    Email = p.Email,
                    Name = p.Name,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AuthorModel> GetAuthorByEMailAndPassword(string email, string password)
        {
            return await _authorRepository.Where(p => p.Email == email && p.Password == password).Select(p => new AuthorModel
            {
                Id = p.Id,
                Email = p.Email,
                Name = p.Name
            }).FirstOrDefaultAsync();
        }

        public async Task Update(int id, AuthorModel author)
        {
            var authorEntity = await _authorRepository.Get(id);

            authorEntity.Name = author.Name;
            authorEntity.Email = author.Email;
            authorEntity.Password = author.Password;

            await _authorRepository.Update(authorEntity);

            _cache.Remove(_cacheKey);
        }
    }
}
