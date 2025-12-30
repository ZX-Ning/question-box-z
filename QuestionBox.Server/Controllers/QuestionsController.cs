using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestionBox.Data;
using QuestionBox.Models;
using System.Text.Json;

namespace QuestionBox.Controllers;

[ApiController]
[Route("/api/questions")]
public sealed class QuestionsController(
    ILogger<QuestionsController> logger,
    QuestionDbContext dbContext
) : ControllerBase {
    public record struct QuestionWrapper(string question);
    public record struct QuestionDto(
        int index,
        string question,
        string answer,
        string questionTime,
        string? answerTime
    );

    [HttpGet]
    public async Task<List<QuestionDto>> Get() {
        List<QuestionModel> data = await dbContext.questions
            .Where(q => q.Answer != null)
            .OrderByDescending(q => q.AnswerTime!)
            .ThenByDescending(q => q.QuestionTime!)
            .ToListAsync();

        logger.LogDebug(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));

        List<QuestionDto> questions = data
            .Select((q, i)
                => new QuestionDto(i, q.Question, q.Answer!, q.QuestionTime, q.AnswerTime)
            )
            .ToList();
        return questions;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] QuestionWrapper question) {
        var date = DateTime.Now;
        string ip;
        if (HttpContext.Request.Headers.TryGetValue("Cf-Connecting-Ip", out var x)) {
            ip = x.ToString();
        }
        else {
            ip = HttpContext.Connection.RemoteIpAddress!.ToString();
        }
        await dbContext.questions.AddAsync(new QuestionModel {
            IpAddr = ip,
            Question = question.question,
            QuestionTime = date.ToString("yyyy-MM-dd"),
        });
        await dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<List<QuestionModel>> GetAllQuestions() {
        return await dbContext.questions.ToListAsync();
    }

}