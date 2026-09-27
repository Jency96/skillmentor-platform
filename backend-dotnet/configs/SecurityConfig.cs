using SkillMentor.security;
namespace SkillMentor.configs;
public static class SecurityConfig
{
    public static void Configure(IServiceCollection services)
    {
        services.AddAuthentication(options => { options.DefaultAuthenticateScheme = AuthenticationFilter.Scheme; options.DefaultChallengeScheme = AuthenticationFilter.Scheme; })
            .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, AuthenticationFilter>(AuthenticationFilter.Scheme, _ => { });
        services.AddAuthorization();
    }
}
