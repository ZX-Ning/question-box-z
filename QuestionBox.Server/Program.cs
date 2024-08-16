using Microsoft.Extensions.FileProviders;
using QuestionBox.Server.DataBase;
using QuestionBoxl;

var rootDir = Directory.GetParent(Directory.GetCurrentDirectory())!.ToString();

var builder = WebApplication.CreateBuilder(new WebApplicationOptions {
    Args = args,
    WebRootPath = Path.Combine(rootDir, "QuestionBox.Client/dist")
});
var fileProvider = new PhysicalFileProvider(rootDir);
var appConfig = new AppConfig(fileProvider);

builder.Services.AddControllers();
builder.Services.AddSingleton<IDataProvider, SqliteDataProvider>();
builder.Services.AddSingleton(appConfig);
builder.Services.AddSingleton<IFileProvider>(fileProvider);

if (appConfig.Config.Mysql.UsingMysql) {
    builder.Services.AddDbContext<QuestionDbContext, QuestionsMariaDbContext>(ServiceLifetime.Singleton);
}
else {
    builder.Services.AddDbContext<QuestionDbContext, QuestionsSqliteDbContext>(ServiceLifetime.Singleton);
}

if (builder.Environment.IsDevelopment()) {
    builder.WebHost.UseUrls(appConfig.Config.ServerDevUrl);
    builder.Services.AddCors(options => {
        options.AddPolicy(
            name: "AllowAll",
            policy => {
                policy
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod();
            });
    });
}
else {
    builder.WebHost.UseUrls(appConfig.Config.ServerProductionUrl);
}

var app = builder.Build();

app.UseCors("AllowAll");
app.MapControllers();
if (!app.Environment.IsDevelopment()) {
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.Run();