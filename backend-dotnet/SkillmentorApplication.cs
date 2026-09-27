using SkillMentor.configs;
using SkillMentor.respositories;
using SkillMentor.security;
using SkillMentor.services;
using SkillMentor.services.impl;
using Microsoft.EntityFrameworkCore;

namespace SkillMentor;

public static class SkillmentorApplication
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.WebHost.UseUrls($"http://0.0.0.0:{builder.Configuration["PORT"] ?? "8081"}");
        builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(OpenApiConfig.Configure);
        builder.Services.AddDbContext<SkillMentorDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<MentorRepository>();
        builder.Services.AddScoped<StudentRepository>();
        builder.Services.AddScoped<SubjectRepository>();
        builder.Services.AddScoped<SessionRepository>();
        builder.Services.AddScoped<MentorService, MentorServiceImpl>();
        builder.Services.AddScoped<StudentService, StudentServiceImpl>();
        builder.Services.AddScoped<SubjectService, SubjectServiceImpl>();
        builder.Services.AddScoped<SessionService, SessionServiceImpl>();
        ValidatorConfiguration.Configure(builder.Services, builder.Configuration);
        CorsConfig.Configure(builder.Services, builder.Configuration);
        RedisConfig.Configure(builder.Services, builder.Configuration);
        SecurityConfig.Configure(builder.Services);
    }

    public static void ConfigurePipeline(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<SkillMentorAuthenticationEntryPoint>();
        app.UseCors(CorsConfig.PolicyName);
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}
