using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using QuestionBox.Server.DataBase;
using QuestionBox.Server.Models;

namespace QuestionBox.AddControllers;

[ApiController]
[Route("/api/questions")]
public class QuestionsController(IDataProvider dataProvider) : ControllerBase {

    [HttpGet]
    public async Task<JsonArray> Get() {
        var questions = await dataProvider.GetAnsweredQuestionsAsync();
        var nodes = questions.Select((q, i) => {
            var jsonObj = JsonSerializer.SerializeToNode(q)!.AsObject();
            jsonObj.Add("index", i);
            return jsonObj;
        })
        .ToArray();
        return new JsonArray(nodes);
    }
    [HttpPost]
    public async void Post([FromBody] Question question) {
        var date = DateTime.Now;
        string ip;
        // Console.WriteLine(JsonSerializer.Serialize(HttpContext.Request.Headers));
        if (HttpContext.Request.Headers.TryGetValue("Cf-Connecting-Ip", out var x)) {
            ip = x.ToString();
        }
        else {
            ip = HttpContext.Connection.RemoteIpAddress!.ToString();
        }
        QuestionWithTime q = new() {
            question = question.question,
            questionTime = date.ToString("yyyy-MM-dd"),
        };
        await dataProvider.AddQuestionAsync(q, ip);
    }
}