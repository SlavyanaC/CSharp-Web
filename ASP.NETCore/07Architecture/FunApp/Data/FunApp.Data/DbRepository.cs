using System;
using System.Linq;
using System.Threading.Tasks;
using FunApp.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace FunApp.Data
{
    public class DbRepository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class
    {
        private readonly FunAppContext context;
        private readonly DbSet<TEntity> dbSet;

        public DbRepository(FunAppContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }

        public IQueryable<TEntity> All() => this.dbSet;

        public Task AddAsync(TEntity entity) => this.dbSet.AddAsync(entity);

        public void Delete(TEntity entity) => this.dbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => this.context.SaveChangesAsync();

        public void Dispose() => this.context.Dispose();
    }
}
