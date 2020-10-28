using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<bool> Insert(TEntity entity);
        Task<TEntity> GetById(object Id);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
    }
}
