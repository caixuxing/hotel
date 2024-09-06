namespace Hotel.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(m =>
        {
            m.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        return services;
    }
}


