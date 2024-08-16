using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuestionBox.Server.Models;

[Table("questions")]
[PrimaryKey(nameof(id))]
public class QuestionDbItem {
    [Key]
    public int id { get; }
    public required string question { get; set; }
    public required string questionTime { get; set; }
    public string? answer { get; set; }
    public string? answerTime { get; set; }
    public string? ipAddr { get; set; }
}
public record struct Question(string question);
public record struct QuestionWithTime(
    string question,
    string questionTime,
    string? answer,
    string? answerTime
);