using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

public enum QuestionType
{
    SingleChoice,
    MultipleChoice,
    TextAnswer
}

[Table("Test_questions")]
public class TestQuestion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("test_id")]
    public int TestId { get; set; }

    [ForeignKey("TestId")]
    public Test Test { get; set; } = null!;

    [Column("question_text")]
    public string QuestionText { get; set; } = string.Empty;

    [Column("question_type")]
    public QuestionType QuestionType { get; set; } = QuestionType.SingleChoice;

    [Column("order_index")]
    public int OrderIndex { get; set; } = 0;

    [Column("points")]
    public byte Points { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public ICollection<TestQuestionOption> Options { get; set; } = new List<TestQuestionOption>();
}