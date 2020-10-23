using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces
{
    public interface IServiceFactory
    {
        TEntity GetServices<TEntity>(params Type[] services) where TEntity : class;

    }
}
