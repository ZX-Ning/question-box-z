namespace QuestionBox;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.FileProviders;
using QuestionBox.Data;
using QuestionBox.Auth;
static class Program {
    static WebApplicationBuilder builder(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // set up root dir
        var rootDir = Directory.GetCurrentDirectory()!;
        if (builder.Environment.IsDevelopment()) {
            rootDir = Directory.GetParent(Directory.GetCurrentDirectory())!.ToString();
        }

        // set up logger
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        ILogger logger = builder.Services
                .BuildServiceProvider().GetService<ILoggerFactory>()!.CreateLogger("Builder");

        logger.LogInformation($"Rootdir: {rootDir}");

        builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(rootDir));
        builder.Configuration.AddJsonFile(
            Path.Combine(rootDir, "AppConfig.jsonc"), optional: false, reloadOnChange: true
        );

        builder.Services.AddSingleton<ILoginChecker, SimpleLoginChecker>();

        var config = builder.Configuration;
        if (config.GetSection("Postgres:usingPg").Get<bool>()) {
            logger.LogInformation("Using configured Postgress database");
            builder.Services.AddDbContext<QuestionDbContext, QuestionsPostgresDbContext>();
        }
        else {
            logger.LogInformation("Using sqlite database with configured file path: " + config["SqlitePath"]);
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
            options.Cookie.SameSite = SameSiteMode.Strict;
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
            builder.Services.AddOpenApi();
        }
        return builder;
    }
    public static void Main(string[] args) {
        WebApplication app = builder(args).Build();
        app.MapControllers();

        using (var scope = app.Services.CreateScope()) {
            var db = scope.ServiceProvider.GetRequiredService<QuestionDbContext>();
            db.onAppStart();
        }

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
            app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
                string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));
            app.UseCors("default");
        }
        if (app.Environment.IsProduction()) {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.MapFallbackToFile("index.html");
        }
        app.UseAuthentication();
        app.UseAuthorization();
        app.Run();
    }
}