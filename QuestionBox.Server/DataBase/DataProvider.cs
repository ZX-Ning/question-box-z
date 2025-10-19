namespace QuestionBox.DataBase;

using Microsoft.EntityFrameworkCore;
using QuestionBox.Models;


public interface IDataProvider {
    public Task<List<QuestionModel>> GetAnsweredQuestionsAsync();
    public Task AddQuestionAsync(QuestionModel question);
}

public abstract class DefaultDataProvider : IDataProvider {
    protected abstract QuestionDbContextBase DbContext { get; }
    public virtual async Task AddQuestionAsync(QuestionModel q) {
        await DbContext.questions.AddAsync(q);
        await DbContext.SaveChangesAsync();
    }
    public virtual async Task<List<QuestionModel>> GetAnsweredQuestionsAsync() {
        List<QuestionModel> questions = await DbContext.questions
            .Where(q => q.Answer != null)
            .OrderByDescending(q => q.AnswerTime!)
            .ThenByDescending(q => q.QuestionTime!)
            .ToListAsync();

        return questions.ToList();
    }
}

public sealed class SqliteDataProvider : DefaultDataProvider {
    QuestionDbContextBase _dbContext;
    protected override QuestionDbContextBase DbContext => _dbContext;
    public SqliteDataProvider(QuestionDbContextBase dbContext) {
        this._dbContext = dbContext;
        dbContext.Database.EnsureCreated();
        dbContext.Database.ExecuteSqlRaw("PRAGMA journal_mode = 'DELETE';");
    }
}

public sealed class MysqlDataProvider : DefaultDataProvider {
    QuestionDbContextBase _dbContext;

    protected override QuestionDbContextBase DbContext => _dbContext;

    public MysqlDataProvider(QuestionDbContextBase dbContext) {
        this._dbContext = dbContext;
        dbContext.Database.EnsureCreated();
    }
}