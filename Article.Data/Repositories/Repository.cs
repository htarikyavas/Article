using Article.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Article.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ArticleContext dbContext;
        public Repository(ArticleContext dbContext)
        {
            this.dbContext = dbContext;
            this.Table = dbContext.Set<T>();
        }
        public DbSet<T> Table { get; set; }

        public async Task<T> Get(int Id)
        {
            return await Table.SingleAsync(a => a.Id == Id);
        }

        public async Task<int> Add(T entity)
        {
            Table.Add(entity);
            await Save();
            return entity.Id;
        }

        public Task<bool> Update(T entity)
        {
            Table.Update(entity);
            return Save();
        }

        public Task<bool> Delete(T entity)
        {
            Table.Remove(entity);
            return Save();
        }

        public IQueryable<T> All()
        {
            return Table;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> where)
        {
            return Table.Where(where);
        }

        public IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc)
        {
            if (isDesc)
                return Table.OrderByDescending(orderBy);
            return Table.OrderBy(orderBy);
        }

        private async Task<bool> Save()
        {
            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                // TODO: Log Exceptions
                return false;
            }
        }
    }
}
