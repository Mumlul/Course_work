using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;

namespace course_work.Models.Classes;


[Table("UserProfile")]
public class UserProfile:ObservableObject
{
    [Key]
    [Column("Id")] public int Id { get; set; }
    [Column("userId")] public int UserId { get; set; }
    [ForeignKey("UserId")] public virtual User User { get; set; } = null!;
    [Column("avatar")] [field: ObservableProperty] public string? Avatar { get; set; } = "https://6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672.s3.twcstorage.ru/handsome.jpg";
    [Column("description")] public string? Description { get; set; }
    [Column("Birthday")] public DateTime? Birthday { get; set; }
}
