using Base.Models;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Base.Utils
{
    public static class Extensions
    {

        public static IServiceCollection AddFreeSql(this IServiceCollection services, DataType dataType, string connectionString, bool isProduction = false)
        {
            IFreeSql freeSql;

            if (isProduction)
            {
                freeSql = new FreeSqlBuilder()
                    .UseConnectionString(DataType.MySql, connectionString)
                    .UseAutoSyncStructure(false)
                    .Build();
            }
            else
            {
                freeSql = new FreeSqlBuilder()
                    .UseConnectionString(DataType.MySql, connectionString)
                    .UseMonitorCommand(command => Console.WriteLine($"SQL: {command.CommandText}"))
                    .UseAutoSyncStructure(true)
                    .Build();

                freeSql.CodeFirst.SyncStructure<Account>();
                freeSql.CodeFirst.SyncStructure<Channel>();
                freeSql.CodeFirst.SyncStructure<ChannelMerchant>();
                freeSql.CodeFirst.SyncStructure<ChannelMerchantOrderSummary>();
                freeSql.CodeFirst.SyncStructure<Customer>();
                freeSql.CodeFirst.SyncStructure<Order>();
                freeSql.CodeFirst.SyncStructure<PaymentRule>();
                freeSql.CodeFirst.SyncStructure<Refund>();
                freeSql.CodeFirst.SyncStructure<Transaction>();
            }

            BaseEntity.Initialization(freeSql, null);

            services.AddSingleton(freeSql);

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(IScoped)) ?? throw new Exception("加载Base程序集错误");
            var allServiceTypes = assembly.GetTypes();

            // 注册服务
            var serviceTypes = allServiceTypes.Where(p => !p.IsAbstract && !p.IsInterface && p.GetInterface(nameof(IScoped)) != null);
            foreach (var serviceType in serviceTypes)
            {
                services.AddScoped(serviceType);
            }
            serviceTypes = allServiceTypes.Where(p => !p.IsAbstract && !p.IsInterface && p.GetInterface(nameof(ISingleton)) != null);
            foreach (var serviceType in serviceTypes)
            {
                services.AddSingleton(serviceType);
            }

            return services;
        }
    }

    public interface IScoped { }

    public interface ISingleton { }

    public interface ITransient { }

}
