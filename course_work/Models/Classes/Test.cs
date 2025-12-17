using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

[Table("Tests")]
public class Test
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("course_id")]
    public int CourseId { get; set; }

    [ForeignKey("CourseId")]
    public Course Course { get; set; } = null!;

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("passing_score")]
    public byte PassingScore { get; set; } = 70;

    [Column("time_limit_minutes")]
    public int? TimeLimitMinutes { get; set; }

    [Column("attempts_allowed")]
    public int AttemptsAllowed { get; set; } = 1;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public ICollection<TestQuestion> Questions { get; set; } = new List<TestQuestion>();
}