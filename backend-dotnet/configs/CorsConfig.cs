namespace SkillMentor.configs;
public static class CorsConfig
{
    public const string PolicyName = "Frontend";
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        var origins = (configuration["CORS_ALLOWED_ORIGINS"] ?? "http://localhost:3000,http://localhost:5173,http://localhost:8080")
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        services.AddCors(options => options.AddPolicy(PolicyName, policy => policy
            .WithOrigins(origins).WithMethods("GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS")
            .AllowAnyHeader().AllowCredentials()
            .WithExposedHeaders("Authorization", "X-Rate-Limit-Remaining", "X-Rate-Limit-Retry-After-Seconds", "Retry-After")
            .SetPreflightMaxAge(TimeSpan.FromHours(1))));
    }
}
