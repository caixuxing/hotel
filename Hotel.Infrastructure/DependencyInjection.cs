using Hotel.Infrastructure.BackgroundTask;

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

