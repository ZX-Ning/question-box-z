namespace QuestionBox.Data;

using Microsoft.EntityFrameworkCore;
using QuestionBox.Models;

public interface IDataProvider {
    public Task<List<QuestionModel>> GetAnsweredQuestionsAsync();
    public Task AddQuestionAsync(QuestionModel question);
}

public class DatabaseDataProvider(QuestionDbContext dbContext) : IDataProvider {
    public virtual async Task AddQuestionAsync(QuestionModel q) {
        await dbContext.questions.AddAsync(q);
        await dbContext.SaveChangesAsync();
    }
    public virtual async Task<List<QuestionModel>> GetAnsweredQuestionsAsync() {
        List<QuestionModel> questions = await dbContext.questions
            .Where(q => q.Answer != null)
            .OrderByDescending(q => q.AnswerTime!)
            .ThenByDescending(q => q.QuestionTime!)
            .ToListAsync();

        return questions;
    }
}