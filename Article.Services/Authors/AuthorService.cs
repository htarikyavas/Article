using System.Collections.Generic;
using System.Threading.Tasks;
using Article.Core.Domain;
using Article.Data.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Article.Services.Authors
{
    public class AuthorService : IAuthorService
    {

        private readonly IRepository<Author> _authorRepository;

        public AuthorService(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<int> Add(AuthorModel author)
        {
            return await _authorRepository.Add(new Author()
            {
                Name = author.Name,
                Email = author.Email
            });
        }

        public async Task<List<AuthorModel>> All()
        {
            return await _authorRepository.All()
                .Select(p => new AuthorModel
                {
                    Id = p.Id,
                    Email = p.Email,
                    Name = p.Name,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task Delete(int id)
        {
            await _authorRepository.Delete(
               await _authorRepository.Get(id)
           );
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

        public async Task Update(int id, AuthorModel author)
        {
            var authorEntity = await _authorRepository.Get(id);

            authorEntity.Name = author.Name;
            authorEntity.Email = author.Email;

            await _authorRepository.Update(authorEntity);
        }
    }
}
