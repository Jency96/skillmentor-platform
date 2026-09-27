using SkillMentor.configs;
using SkillMentor;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
SkillmentorApplication.Configure(builder);
var app = builder.Build();
SkillmentorApplication.ConfigurePipeline(app);
app.Run();
public partial class Program { }
