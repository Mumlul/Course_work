using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

[Table("Courses")]
public class Course
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("slug")]
    public string Slug { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("preview_image", TypeName = "BLOB")]
    public byte[]? PreviewImage { get; set; }

    public ICollection<Module> Modules { get; set; } = new List<Module>();
    public ICollection<CourseAuthors> Authors { get; set; } = new List<CourseAuthors>();
    public ICollection<CourseStudents> Students { get; set; } = new List<CourseStudents>();
    public ICollection<Test> Tests { get; set; } = new List<Test>();
    public ICollection<CourseReview> Reviews { get; set; } = new List<CourseReview>();
}