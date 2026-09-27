namespace SkillMentor.security;
public interface TokenValidator
{
    Task<bool> ValidateToken(string token);
    string? ExtractUserId(string token);
    List<string> ExtractRoles(string token);
    string? ExtractFirstName(string token);
    string? ExtractLastName(string token);
    string? ExtractEmail(string token);
}
