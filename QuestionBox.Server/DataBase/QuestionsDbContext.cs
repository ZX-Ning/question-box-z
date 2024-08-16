namespace QuestionBox.Server.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using QuestionBox.Server.Models;
using QuestionBoxl;

public abstract class QuestionDbContext : DbContext {
    public abstract DbSet<QuestionDbItem> questions { get; set; }
}

public class QuestionsSqliteDbContext(
    IFileProvider fileProvider,
    AppConfig appConfig
) : QuestionDbContext {
    public override DbSet<QuestionDbItem> questions { get; set; }
    public string DbPath { get => fileProvider.GetFileInfo(appConfig.Config.SqlitePath).PhysicalPath!; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite($"Data Source={DbPath};");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<QuestionDbItem>();
    }
}

public class QuestionsMariaDbContext (AppConfig appConfig) : QuestionDbContext {
    public override DbSet<QuestionDbItem> questions { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        var config = appConfig.Config.Mysql;
        optionsBuilder
            .UseMySql(
                $"server={config.server};port={config.port};user={config.user};password={config.password};database={config.db}",
                new MariaDbServerVersion("10.6.18")
            );
    }
}