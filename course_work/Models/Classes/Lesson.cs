using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

public enum LessonType
{
    Text,
    Video,
    Quiz
}

[Table("Lessons")]
public class Lesson
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("module_id")]
    public int ModuleId { get; set; }

    [ForeignKey("ModuleId")]
    public Module Module { get; set; } = null!;

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("slug")]
    public string Slug { get; set; } = string.Empty;

    [Column("order_index")]
    public int OrderIndex { get; set; } = 0;

    [Column("content_url")]
    public string ContentUrl { get; set; } = string.Empty;

    [Column("lesson_type")]
    public LessonType LessonType { get; set; } = LessonType.Text;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updates_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("preview_image")]
    public string PreviewImage { get; set; } =
        "https://6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672.s3.twcstorage.ru/lesson.jpg";
}