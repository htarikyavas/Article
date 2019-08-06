using Article.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Article.Services.Authors
{
    public interface IAuthorService
    {
        Task<AuthorModel> Get(int id);

        Task Delete(int id);

        Task<List<AuthorModel>> All();

        Task Update(int id, AuthorModel author);

        Task<int> Add(AuthorModel author);
    }
}
