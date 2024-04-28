namespace InvestList
{
    public static class ConfigExtension
    {
        public static WebApplicationBuilder LoadAppSettingAndEnvValues(this WebApplicationBuilder configBuilder,  string? env = null)
        {
            configBuilder.Configuration
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    true, true)
                .AddJsonFile("appsettings.private.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            
            return configBuilder;
        }
    }
}