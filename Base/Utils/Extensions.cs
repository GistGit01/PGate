using Base.Models;
using FreeSql;
using Microsoft.AspNetCore.Builder;
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

        public static void InitTestData(this IApplicationBuilder app, IFreeSql freeSql)
        {
            var sql = @"
SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;


-- ----------------------------
-- Records of channel
-- ----------------------------
INSERT INTO `channel` VALUES (1, 'OMIPAY', '2025-07-05 11:32:32.000', '2025-07-05 11:32:34.000', 1);

-- ----------------------------
-- Records of channel_merchant
-- ----------------------------
INSERT INTO `channel_merchant` VALUES (1, 'RR Surfer', 1, 'OMIPAY', 'c6c86d21d2c140a08e2b41c789aec5ae', '000034', 1, '2025-07-05 11:34:03.000', '2025-07-05 11:34:05.000', 1);

-- ----------------------------
-- Records of customer
-- ----------------------------
INSERT INTO `customer` VALUES (1, 'TEST', 'test@test.com', 'abc.1234', '86497200595011f098b100ffccae5c0a', '2025-07-05 11:31:48.000', '2025-07-05 11:31:56.000', 1);


SET FOREIGN_KEY_CHECKS = 1;
";
            freeSql.Ado.ExecuteNonQuery(sql);
        }
    }

    public interface IScoped { }

    public interface ISingleton { }

    public interface ITransient { }

}
