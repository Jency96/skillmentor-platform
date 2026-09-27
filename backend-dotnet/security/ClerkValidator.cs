using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
namespace SkillMentor.security;
public class ClerkValidator(string jwksUrl) : TokenValidator
{
    private static readonly HttpClient Client = new();
    public async Task<bool> ValidateToken(string token)
    {
        try
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            if (jwt.Header.Kid is null || jwt.Header.Alg != SecurityAlgorithms.RsaSha256) return false;
            var jwks = new JsonWebKeySet(await Client.GetStringAsync(jwksUrl));
            var key = jwks.GetSigningKeys().FirstOrDefault(k => k.KeyId == jwt.Header.Kid);
            if (key is null) return false;
            new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = key, ValidateIssuerSigningKey = true,
                ValidateIssuer = false, ValidateAudience = false, ValidateLifetime = true,
                RequireSignedTokens = true, ValidAlgorithms = [SecurityAlgorithms.RsaSha256], ClockSkew = TimeSpan.FromMinutes(5)
            }, out _);
            return true;
        }
        catch { return false; }
    }
    private static JwtSecurityToken Decode(string token) => new JwtSecurityTokenHandler().ReadJwtToken(token);
    private static string? Claim(string token, string key) => Decode(token).Claims.FirstOrDefault(x => x.Type == key)?.Value;
    public string? ExtractUserId(string token) => Claim(token, "sub");
    public string? ExtractEmail(string token) => Claim(token, "email");
    public string? ExtractFirstName(string token) => Claim(token, "firstName");
    public string? ExtractLastName(string token) => Claim(token, "lastName");
    public List<string> ExtractRoles(string token)
    {
        try
        {
            var raw = Claim(token, "roles");
            return raw is null ? [] : System.Text.Json.JsonSerializer.Deserialize<List<string>>(raw) ?? [];
        }
        catch { return []; }
    }
}
