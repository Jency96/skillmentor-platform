using Microsoft.OpenApi.Models;
namespace SkillMentor.configs;
public static class OpenApiConfig
{
    public static void Configure(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Skill Mentor API", Version = "1.0", Description = "Skill Mentor Platform Service API Documentation", Contact = new OpenApiContact { Name = "Skill Mentor Support", Email = "support@skillmentor.com" }, License = new OpenApiLicense { Name = "Apache 2.0" } });
        options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme { Type = SecuritySchemeType.Http, Scheme = "bearer", BearerFormat = "JWT" });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement { [new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" } }] = Array.Empty<string>() });
    }
}
