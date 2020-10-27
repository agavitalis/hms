using HMS.Database;
using HMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMS.Services.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext db;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ApplicationDbContext applicationDbContext)
        {
            db = applicationDbContext;
            this.dbSet = applicationDbContext.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }


        public async Task<TEntity> GetById(object Id) =>
            await dbSet.FindAsync(Id);
        
        public async Task<bool> Insert(TEntity entity)
        {
            try
            {
                dbSet.Add(entity);

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
