namespace WebApplication1.Extensions
{
    public static class ConfigExtensions
    {
        public static IServiceCollection Load<T>(this IServiceCollection services, ConfigurationManager manager, string sectionName)
        where T : class
        {
            var section = manager.GetSection(sectionName) ?? throw new NullReferenceException($"{sectionName} is load with null value");
            services.Configure<T>(section);
            return services;
        }
    }
}
