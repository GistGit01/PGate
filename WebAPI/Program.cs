
using Base.Services.Orders;
using Base.Utils;
using FreeSql;
using FSqlRepositories;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAPI.Utils;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(config =>
            {
                config.Filters.Add<ExceptionFilter>();
            }).AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("*", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            BootStrapper.ConfigServices(builder.Services, builder.Configuration, builder.Environment.IsProduction());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }

    public class BootStrapper
    {
        public static void ConfigServices(IServiceCollection services, IConfiguration configuration, bool isProduction)
        {
            services.AddSingleton<IConfiguration>(configuration);

            services.AddFreeSql(DataType.MySql, configuration.GetConnectionString("MySqlMasterDatabase"), isProduction);

            services.AddRepositories();

            services.AddServices();
        }
    }
}
