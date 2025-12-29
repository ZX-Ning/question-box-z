namespace QuestionBox;

using Microsoft.Extensions.FileProviders;
using QuestionBox.Data;

static class Program {
    static WebApplicationBuilder builder() {
        var rootDir = Directory.GetParent(Directory.GetCurrentDirectory())!.ToString();

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions {
            WebRootPath = Path.Combine(rootDir, "QuestionBox.Client/dist")
        });
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        var fileProvider = new PhysicalFileProvider(rootDir);
        // var appConfig = new AppConfig(fileProvider);

        builder.Configuration.AddJsonFile(Path.Combine(rootDir, "AppConfig.json"), optional: false, reloadOnChange: true);
        builder.Services.AddSingleton<IFileProvider>(fileProvider);

        var config = builder.Configuration;
        ILogger logger = builder.Services
                        .BuildServiceProvider().GetService<ILoggerFactory>()!.CreateLogger("Builder");

        if (config.GetSection("Postgres:UsingPg").Get<bool>()) {
            logger.LogInformation("Using configured Postgress database");
            builder.Services.AddDbContext<QuestionDbContext, QuestionsPostgresDbContext>();
        }
        else {
            logger.LogInformation("Using sqlite database with configured file path");
            builder.Services.AddDbContext<QuestionDbContext, QuestionsSqliteDbContext>();
        }
        builder.Services.AddControllers();

        //allow cors when in development mode
        if (builder.Environment.IsDevelopment()) {
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
            builder.WebHost.UseUrls(config["ServerDevUrl"]!);
            // builder.Services.AddOpenApi();
        }
        if (builder.Environment.IsProduction()) {
            builder.WebHost.UseUrls(config["ServerProductionUrl"]!);
        }
        return builder;
    }
    public static void Main(/*string[] args*/) {
        WebApplication app = builder().Build();
        app.MapControllers();
        app.UseCors("AllowAll");

        using (var scope = app.Services.CreateScope()) {
            var db = scope.ServiceProvider.GetRequiredService<QuestionDbContext>();
            db.onAppStart();
        }

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }
        if (app.Environment.IsProduction()) {
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
        app.Logger.LogInformation("Starting Server, Environment: " + app.Environment.EnvironmentName);
        app.Run();
    }
}