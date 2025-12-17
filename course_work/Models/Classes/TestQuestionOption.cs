using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

[Table("Test_question_options")]
public class TestQuestionOption
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("question_id")]
    public int QuestionId { get; set; }

    [ForeignKey("QuestionId")]
    public TestQuestion Question { get; set; } = null!;

    [Column("option_text")]
    public string OptionText { get; set; } = string.Empty;

    [Column("is_correct")]
    public bool IsCorrect { get; set; } = false;

    [Column("order_index")]
    public int OrderIndex { get; set; } = 0;
}