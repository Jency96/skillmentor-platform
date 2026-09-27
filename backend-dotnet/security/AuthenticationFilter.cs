using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
namespace SkillMentor.security;
public class AuthenticationFilter(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, TokenValidator validator)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string Scheme = "SkillMentorBearer";
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorization = Request.Headers.Authorization.ToString();
        if (!authorization.StartsWith("Bearer ", StringComparison.Ordinal)) return AuthenticateResult.NoResult();
        var token = authorization[7..];
        if (!await validator.ValidateToken(token)) return AuthenticateResult.Fail("Invalid token");
        try
        {
            var claims = new List<Claim>();
            void Add(string type, string? value) { if (value is not null) claims.Add(new Claim(type, value)); }
            Add(ClaimTypes.NameIdentifier, validator.ExtractUserId(token));
            Add(ClaimTypes.Email, validator.ExtractEmail(token));
            Add(ClaimTypes.GivenName, validator.ExtractFirstName(token));
            Add(ClaimTypes.Surname, validator.ExtractLastName(token));
            foreach (var role in validator.ExtractRoles(token)) Add(ClaimTypes.Role, role);
            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme)), Scheme));
        }
        catch { return AuthenticateResult.Fail("Invalid token claims"); }
    }
    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = 401;
        return Response.WriteAsJsonAsync(new { status = 401, error = "Unauthorized or Token has expired" });
    }
}
