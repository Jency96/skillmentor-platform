using SkillMentor.security;
namespace SkillMentor.configs;
public static class ValidatorConfiguration
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration["AUTH_VALIDATOR_TYPE"] == "stem-link")
            services.AddSingleton<TokenValidator>(_ => new SkillMentorJwtValidator(configuration["JWT_SECRET"] ?? throw new InvalidOperationException("JWT_SECRET is required")));
        else
            services.AddSingleton<TokenValidator>(_ => new ClerkValidator(configuration["CLERK_JWKS_URL"] ?? throw new InvalidOperationException("CLERK_JWKS_URL is required")));
    }
}
