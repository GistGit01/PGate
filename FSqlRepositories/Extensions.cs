using Base.Utils;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSqlRepositories
{
    public static class Extensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            //services.AddScoped(typeof(IBaseRepository<>), typeof(GuidRepository<>));
            //services.AddScoped(typeof(BaseRepository<>), typeof(GuidRepository<>));

            services.AddScoped(typeof(IBaseRepository<,>), typeof(DefaultRepository<,>));
            services.AddScoped(typeof(BaseRepository<,>), typeof(DefaultRepository<,>));

            var assembly = typeof(AccountRepository).Assembly;
            var iScopeInterfaceName = nameof(IScoped);

            foreach (var repo in assembly.GetTypes().Where(a => a.IsAbstract == false && typeof(IBaseRepository).IsAssignableFrom(a)))
            {
                var repositoryInterfaceType = repo.GetInterfaces().FirstOrDefault(p => p.GetInterface(iScopeInterfaceName) != null);
                if (repositoryInterfaceType != null)
                    services.AddScoped(repositoryInterfaceType, repo);
            }
            return services;
        }
    }
}
