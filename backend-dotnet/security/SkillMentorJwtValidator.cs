using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace SkillMentor.security;
public class SkillMentorJwtValidator(string secretKey) : TokenValidator
{
    public Task<bool> ValidateToken(string token)
    {
        try
        {
            new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuerSigningKey = true, ValidateIssuer = false, ValidateAudience = false,
                ValidateLifetime = true, ValidAlgorithms = [SecurityAlgorithms.HmacSha256]
            }, out _);
            return Task.FromResult(true);
        }
        catch { return Task.FromResult(false); }
    }
    private static string? Claim(string token, string key) => new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.FirstOrDefault(x => x.Type == key)?.Value;
    public string? ExtractUserId(string token) => Claim(token, "sub");
    public string? ExtractFirstName(string token) => null;
    public string? ExtractLastName(string token) => null;
    public string? ExtractEmail(string token) => null;
    public List<string> ExtractRoles(string token)
    {
        try
        {
            using var json = System.Text.Json.JsonDocument.Parse(Base64UrlEncoder.DecodeBytes(token.Split('.')[1]));
            return json.RootElement.TryGetProperty("roles", out var roles) && roles.ValueKind == System.Text.Json.JsonValueKind.Array
                ? roles.EnumerateArray().Select(x => x.GetString() ?? "").ToList() : [];
        }
        catch { return []; }
    }
}
