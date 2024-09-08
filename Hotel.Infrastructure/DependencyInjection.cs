﻿using Hotel.Domain.Shared.Appsettings;
using Hotel.Domain.Shared.Config;
using Hotel.Infrastructure.BackgroundTask;
using Hotel.Infrastructure.Data;
using Snowflake.Core;

namespace Hotel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var connectionConfigs = configuration.GetSection("ConnectionConfigs").Get<IocConfig>();
        services.AddHttpContextAccessor();
        services.AddSingleton<ISqlSugarClient>(s =>
        {
            SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = DbType.SqlServer,
                ConnectionString = connectionConfigs?.ConnectionString ?? "",
                IsAutoCloseConnection = true,
            },
           db =>
           {
               db.Aop.OnLogExecuting = (sql, pars) =>
               {
                   var Strsql= new KeyValuePair<string, SugarParameter[]>(sql, pars);
               };
           });
            return sqlSugar;
        });
        services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>)); // 仓储注册


        services.AddScoped<IBookRepository, BookRepository>();

        var mongodbConfig = Appsetting.Instance.GetSection("MongoDb").Get<MongoDbConfig>();
        services.AddSingleton<MongoDbContext>(new MongoDbContext(mongodbConfig!.ConnectionString, mongodbConfig.DatabaseName));



        // 注册雪花算法ID生成器为单例
        services.AddSingleton<IdWorker>(new IdWorker(1, 1));

        services.AddHostedService<HotelTask>();


        // 注入HttpClient
        services.AddHttpClient("zicp", config => 
        {
            config.BaseAddress = new Uri("http://829rv60706.zicp.fun");
            config.DefaultRequestHeaders.Add("key", "1qaz2wsx");
            config.DefaultRequestHeaders.Add("Value", "vp-152733909-001");
        });
        services.AddHttpClient();
        return services;
    }
}

