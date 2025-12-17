using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

[Table("Test_results")]
public class TestResult
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("test_id")]
    public int TestId { get; set; }

    [ForeignKey("TestId")]
    public Test Test { get; set; } = null!;

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [Column("score")]
    public decimal Score { get; set; }

    [Column("passed")]
    public bool Passed { get; set; }

    [Column("attempt_number")]
    public int AttemptNumber { get; set; } = 1;

    [Column("started_at")]
    public DateTime StartedAt { get; set; }

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }

    [Column("time_spent_seconds")]
    public int? TimeSpentSeconds { get; set; }

    [Column("answers_json", TypeName = "json")]
    public string? AnswersJson { get; set; }
}