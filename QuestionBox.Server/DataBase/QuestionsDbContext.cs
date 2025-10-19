namespace QuestionBox.DataBase;

using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using QuestionBox.Models;

public abstract class QuestionDbContextBase : DbContext {
    public abstract DbSet<QuestionModel> questions { get; set; }
}

public sealed class QuestionsSqliteDbContext(
    IFileProvider fileProvider,
    AppConfig appConfig
) : QuestionDbContextBase {
    public override DbSet<QuestionModel> questions { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        string dbPath = fileProvider
            .GetFileInfo(appConfig.Config["SqlitePath"]!.GetValue<string>())
            .PhysicalPath!;
        optionsBuilder
            .UseSqlite($"Data Source={dbPath}")
            .UseLowerCaseNamingConvention();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<QuestionModel>();
    }
}

public sealed class QuestionsMysqlDbContext(AppConfig appConfig) : QuestionDbContextBase {
    public override DbSet<QuestionModel> questions { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        JsonNode config = appConfig.Config["Mysql"]!;
        optionsBuilder
            .UseMySQL(string.Concat(
                $"server={config["server"]};",
                $"port={config["port"]};",
                $"user={config["user"]};",
                $"password={config["password"]};",
                $"database={config["db"]};"
            ))
            .UseLowerCaseNamingConvention();
    }
}