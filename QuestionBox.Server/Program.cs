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
        var appConfig = new AppConfig(fileProvider);
        builder.Services.AddSingleton(appConfig);
        builder.Services.AddSingleton<IFileProvider>(fileProvider);

        if (appConfig.Config["Postgres"]!["UsingPg"]!.GetValue<bool>()) {
            builder.Services.AddDbContext<QuestionDbContext, QuestionsPostgresDbContext>();
        }
        else {
            builder.Services.AddDbContext<QuestionDbContext, QuestionsSqliteDbContext>();
        }
        builder.Services.AddScoped<IDataProvider, DatabaseDataProvider>();

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
            builder.WebHost.UseUrls(appConfig.Config["ServerDevUrl"]!.GetValue<string>());
            // builder.Services.AddOpenApi();
        }
        if (builder.Environment.IsProduction()) {
            builder.WebHost.UseUrls(appConfig.Config["ServerProductionUrl"]!.GetValue<string>());
        }
        return builder;
    }

    public static void Main(/*string[] args*/) {
        WebApplication app = builder().Build();
        app.MapControllers();
        app.UseCors("AllowAll");

        using (var scope = app.Services.CreateScope()) {
            var db = scope.ServiceProvider.GetRequiredService<QuestionDbContext>();
            db.postAction();
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