using System.Security.Claims;
namespace SkillMentor.security;
public class UserPrincipal
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public static UserPrincipal FromClaims(ClaimsPrincipal user) => new()
    {
        Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value, Email = user.FindFirst(ClaimTypes.Email)?.Value,
        FirstName = user.FindFirst(ClaimTypes.GivenName)?.Value, LastName = user.FindFirst(ClaimTypes.Surname)?.Value
    };
}
