using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

public class Module
{
    [Key]
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int OrderIndex { get; set; }

    public string PreviewImage { get; set; } =
        "https://6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672.s3.twcstorage.ru/module.jpg";

    public Course Course { get; set; } = null!;
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}