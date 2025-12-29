namespace QuestionBox.Data;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using QuestionBox.Models;

public abstract class QuestionDbContext : DbContext {
    public abstract required DbSet<QuestionModel> questions { get; set; }
    public virtual void onAppStart() {
        // do nothing default
    }
}

public sealed class QuestionsSqliteDbContext(
    IFileProvider fileProvider,
    IConfiguration config
) : QuestionDbContext {
    public override required DbSet<QuestionModel> questions { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        string? path = config["SqlitePath"];
        ArgumentNullException.ThrowIfNull(path);
        string dbPath = fileProvider
            .GetFileInfo(path)
            .PhysicalPath!;
        optionsBuilder
            .UseSqlite($"Data Source={dbPath}")
            .UseLowerCaseNamingConvention();
    }
    public override void onAppStart() {
        Database.EnsureCreated();
        Database.ExecuteSqlRaw("PRAGMA journal_mode = 'DELETE';");
        Database.ExecuteSqlRaw("PRAGMA encoding = \"UTF-8\"");
    }

}

public sealed class QuestionsPostgresDbContext(IConfiguration config) : QuestionDbContext {
    public override required DbSet<QuestionModel> questions { get; set; }
    private record struct PgConfig(
        bool UsingPg,
        string server,
        int port,
        string user,
        string password,
        string db
    ) {
        public readonly string connectionStr =>
            $"Host={server};Port={port};Database={db};Username={user};Password={password}";
    };

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        PgConfig pgConfig = config.GetSection("Postgres").Get<PgConfig>();
        optionsBuilder
            .UseNpgsql(pgConfig.connectionStr)
            .UseLowerCaseNamingConvention();
    }
    
    public override void onAppStart() {
        Database.EnsureCreated();
    }
}