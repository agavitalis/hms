using HMS.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Repositories
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider provider;

        public ServiceFactory(IServiceProvider _provider)
        {
            provider = _provider;
        }

        public TEntity GetServices<TEntity>(params Type[] services) where TEntity : class
        {
            var service = provider.GetService<TEntity>();
            return (TEntity)service;
        }
    }
}
