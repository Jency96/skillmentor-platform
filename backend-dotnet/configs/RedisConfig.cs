namespace SkillMentor.configs;
public static class RedisConfig
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        if (bool.TryParse(configuration["CACHE_ENABLED"], out var enabled) && enabled)
        {
            var redis = configuration["REDIS_CONNECTION"] ?? throw new InvalidOperationException("REDIS_CONNECTION required when CACHE_ENABLED=true");
            services.AddStackExchangeRedisCache(options => options.Configuration = redis);
        }
        else services.AddDistributedMemoryCache();
    }
}
