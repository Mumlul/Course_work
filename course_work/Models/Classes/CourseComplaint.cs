using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

[Table("Course_complaints")]
public class CourseComplaint
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("course_id")]
    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;

    [Column("user_id")]
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    [Required]
    [Column("complaint_text")]
    public string ComplaintText { get; set; } = null!;
    
    [Column("fix_days")]
    public int? FixDays { get; set; }

    [Column("in_progress")]
    public bool InProgress { get; set; } = false;

    [Column("is_resolved")]
    public bool IsResolved { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}