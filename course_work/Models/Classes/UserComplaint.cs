using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

[Table("User_complaints")]
public class UserComplaint
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("from_user_id")]
    public int FromUserId { get; set; }

    [ForeignKey(nameof(FromUserId))]
    public User FromUser { get; set; } = null!;

    [Column("to_user_id")]
    public int ToUserId { get; set; }

    [ForeignKey(nameof(ToUserId))]
    public User ToUser { get; set; } = null!;

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