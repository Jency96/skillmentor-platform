using System.Text;
using SkillMentor.security;
using Xunit;
namespace SkillMentor.Tests;
public class TokenValidatorTest
{
    [Fact]
    public void ClerkValidator_Extracts_Array_Of_Roles()
    {
        var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"sub\":\"u1\",\"roles\":[\"ADMIN\",\"STUDENT\"]}"))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        var validator = new ClerkValidator("https://example.com/.well-known/jwks.json");
        Assert.Equal(new[] { "ADMIN", "STUDENT" }, validator.ExtractRoles($"e30.{payload}.signature"));
    }
}
