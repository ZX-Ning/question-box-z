namespace QuestionBox;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.FileProviders;
using QuestionBox.Data;

static class Program {
    static WebApplicationBuilder builder(string[] args) {
        var rootDir = Directory.GetParent(Directory.GetCurrentDirectory())!.ToString();

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions {
            Args = args,
            WebRootPath = Path.Combine(rootDir, "dist"),
        });
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(rootDir));
        builder.Configuration.AddJsonFile(
            Path.Combine(rootDir, "AppConfig.jsonc"), optional: false, reloadOnChange: true
        );
        builder.Services.AddSingleton<LoginChecker>();

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

        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/var/aspnet/keys/"))
            .SetApplicationName("Question-Box-Z"); ;

        builder.Services.AddAuthentication(
            CookieAuthenticationDefaults.AuthenticationScheme
        ).AddCookie(options => {
            options.ExpireTimeSpan = TimeSpan.FromHours(1);
            options.SlidingExpiration = true;
            options.Cookie.HttpOnly = true;
            options.Events.OnRedirectToLogin = context => {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context => {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        });

        builder.Services.AddControllers();

        //allow cors when in development mode
        if (builder.Environment.IsDevelopment()) {
            builder.Services.AddCors(options => {
                options.AddPolicy(
                    name: "default",
                    policy => {
                        policy
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
            builder.WebHost.UseUrls(config["ServerDevUrl"]!);
            builder.Services.AddOpenApi();
        }
        if (builder.Environment.IsProduction()) {
            builder.WebHost.UseUrls(config["ServerProductionUrl"]!);
        }
        return builder;
    }
    public static void Main(string[] args) {
        WebApplication app = builder(args).Build();
        app.MapControllers();
        app.UseCors("default");

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
        app.UseAuthentication();
        app.UseAuthorization();
        app.Logger.LogInformation("Starting Server, Environment: " + app.Environment.EnvironmentName);
        app.Run();
    }
}