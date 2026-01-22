using System;

namespace course_work.Models.Classes;

public class LessonProgress
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int LessonId { get; set; }
    public Lesson Lesson { get; set; }

    public bool Completed { get; set; }

    public DateTime? CompletedAt { get; set; }
}