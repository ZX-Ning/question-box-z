namespace QuestionBox.Server.DataBase;
using Microsoft.EntityFrameworkCore;
using QuestionBox.Server.Models;


public interface IDataProvider {
    public Task<QuestionWithTime[]> GetAnsweredQuestionsAsync();
    public Task AddQuestionAsync(QuestionWithTime question, string? ipAddr);
}

public class SqliteDataProvider: IDataProvider {
    QuestionDbContext dbContext;
    public SqliteDataProvider(QuestionDbContext dbContext) {
        this.dbContext = dbContext;
        dbContext.Database.EnsureCreated();
        dbContext.Database.GetDbConnection();
    }
    public async Task AddQuestionAsync(QuestionWithTime q, string? ipAddr) {
        await dbContext.questions.AddAsync(new QuestionDbItem() {
            question = q.question,
            answer = q.answer,
            questionTime = q.questionTime,
            answerTime = q.answerTime,
            ipAddr = ipAddr
        });
        await dbContext.SaveChangesAsync();
    }
    public Task<QuestionWithTime[]> GetAnsweredQuestionsAsync() {
        return dbContext.questions
            .Where(q => q.answer != null)
            .OrderByDescending(q => q.answerTime!)
            .ThenByDescending(q => q.questionTime)
            .Select(q => new QuestionWithTime(
                q.question,
                q.questionTime,
                q.answer!,
                q.answerTime!
            ))
            .ToArrayAsync();
    }
}