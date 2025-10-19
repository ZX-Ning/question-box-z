using Microsoft.AspNetCore.Mvc;
using QuestionBox.DataBase;
using QuestionBox.Models;

namespace QuestionBox.Controllers;

[ApiController]
[Route("/api/questions")]
public sealed class QuestionsController(IDataProvider dataProvider) : ControllerBase {
    public record struct QuestionWrapper(string question);
    public record struct QuestionDto(
        int index,
        string question,
        string answer,
        string questionTime,
        string? answerTime
    );

    [HttpGet]
    public async Task<IEnumerable<QuestionDto>> Get() {
        List<QuestionModel> data = await dataProvider.GetAnsweredQuestionsAsync();
        return data
            .Select((q, i)
                => new QuestionDto(i, q.Question, q.Answer!, q.QuestionTime, q.AnswerTime)
            );
    }

    [HttpPost]
    public async void Post([FromBody] QuestionWrapper question) {
        var date = DateTime.Now;
        string ip;
        // Console.WriteLine(JsonSerializer.Serialize(HttpContext.Request.Headers));
        if (HttpContext.Request.Headers.TryGetValue("Cf-Connecting-Ip", out var x)) {
            ip = x.ToString();
        }
        else {
            ip = HttpContext.Connection.RemoteIpAddress!.ToString();
        }
        QuestionModel q = new() {
            IpAddr = ip,
            Question = question.question,
            QuestionTime = date.ToString("yyyy-MM-dd"),
        };
        await dataProvider.AddQuestionAsync(q);
    }
}