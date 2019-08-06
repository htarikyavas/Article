using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Article.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> Table { get; }

        Task<T> Get(int Id);
        Task<int> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);

        IQueryable<T> All();
        IQueryable<T> Where(Expression<Func<T, bool>> where);
        IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc);
    }
}
