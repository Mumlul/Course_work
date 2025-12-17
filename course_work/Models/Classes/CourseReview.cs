using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

[Table("Course_reviews")]
public class CourseReview
{
    [Column("id")]
    public int Id { get; set; }

    [Column("course_id")]
    public int CourseId { get; set; }

    [ForeignKey("CourseId")]
    public Course Course { get; set; } = null!;

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [Column("rating")]
    public byte Rating { get; set; }

    [Column("review_text")]
    public string? ReviewText { get; set; }

    [Column("is_approved")]
    public bool IsApproved { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}