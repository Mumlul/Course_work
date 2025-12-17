using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

[Table("Course_students")]
public class CourseStudents
{
    [Key]
    public int Id { get; set; }
    
    [Column("course_id")]
    public int CourseId { get; set; }

    [ForeignKey("CourseId")]
    public Course Course { get; set; } = null!;

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [Column("progress_percent")]
    public byte ProgressPercent { get; set; } = 0;

    [Column("completed")]
    public bool Completed { get; set; } = false;

    [Column("started_at")]
    public DateTime StartedAt { get; set; }

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }
}