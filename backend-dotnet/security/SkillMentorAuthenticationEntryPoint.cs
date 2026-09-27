using SkillMentor.dto;
using SkillMentor.exceptions;
namespace SkillMentor.security;
public class SkillMentorAuthenticationEntryPoint(RequestDelegate next, ILogger<SkillMentorAuthenticationEntryPoint> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (SkillMentorException ex)
        {
            context.Response.StatusCode = ex.Status;
            await context.Response.WriteAsJsonAsync(new ErrorResponse { Message = ex.Message, ErrorCode = ex.Status.ToString() });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new ErrorResponse { Message = "An unexpected error occurred", ErrorCode = "INTERNAL SERVER ERROR" });
        }
    }
}
