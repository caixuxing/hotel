using Microsoft.Extensions.Configuration;


namespace Hotel.Domain.Shared.Appsettings
{
    public class Appsetting
    {
        private static readonly Lazy<IConfigurationRoot> _instance = new Lazy<IConfigurationRoot>(() =>
        {
#if !DEBUG
               Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
#endif
            return new ConfigurationBuilder().AddJsonFile("appsettings.json")
             .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
             .Build();
        });

        private Appsetting()
        { }

        public static IConfigurationRoot Instance
        { get { return _instance.Value; } }
    }
}
