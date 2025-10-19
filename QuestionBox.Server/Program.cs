using Microsoft.Extensions.FileProviders;
using QuestionBox;
using QuestionBox.DataBase;

var rootDir = Directory.GetParent(Directory.GetCurrentDirectory())!.ToString();

var builder = WebApplication.CreateBuilder(new WebApplicationOptions {
    Args = args,
    WebRootPath = Path.Combine(rootDir, "QuestionBox.Client/dist")
});
var fileProvider = new PhysicalFileProvider(rootDir);
var appConfig = new AppConfig(fileProvider);

builder.Services.AddSingleton(appConfig);
builder.Services.AddSingleton<IFileProvider>(fileProvider);

if (appConfig.Config["Mysql"]!["UsingMysql"]!.GetValue<bool>()) {
    builder.Services.AddDbContext<QuestionDbContextBase, QuestionsMysqlDbContext>(ServiceLifetime.Singleton);
    builder.Services.AddSingleton<IDataProvider, MysqlDataProvider>();
}
else {
    builder.Services.AddDbContext<QuestionDbContextBase, QuestionsSqliteDbContext>(ServiceLifetime.Singleton);
    builder.Services.AddSingleton<IDataProvider, SqliteDataProvider>();
}

builder.Services.AddControllers();

//allow cors when in development mode
if (builder.Environment.IsDevelopment()) {
    builder.WebHost.UseUrls(appConfig.Config["ServerDevUrl"]!.GetValue<string>());
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
    builder.Services.AddOpenApi();
}
if (builder.Environment.IsProduction()) {
    builder.WebHost.UseUrls(appConfig.Config["ServerProductionUrl"]!.GetValue<string>());
}

var app = builder.Build();
app.MapControllers();
app.UseCors();

if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}
if (app.Environment.IsProduction()) {
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.Run();