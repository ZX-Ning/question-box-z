using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuestionBox.Models;

[Table("questions")]
[PrimaryKey(nameof(Id))]
public class QuestionModel {
    [Key]
    public int Id { get; }
    public required string Question { get; init; }
    public required string QuestionTime { get; init; }
    public string? Answer { get; init; }
    public string? AnswerTime { get; init; }
    public string? IpAddr { get; init; }
}