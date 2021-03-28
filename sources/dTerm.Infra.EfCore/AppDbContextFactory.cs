using Microsoft.Extensions.DependencyInjection;
using System;

namespace dTerm.Infra.EfCore
{
    public class AppDbContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AppDbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public AppDbContext Create() => _serviceProvider.GetService<AppDbContext>();
    }
}
