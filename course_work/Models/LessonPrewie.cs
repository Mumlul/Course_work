using course_work.Models.Classes;

namespace course_work.Models;

public class LessonPrewie
{
    public Lesson Lesson { get; set; }
    public bool IsCompleted { get; set; }
    public string CompletedText => IsCompleted ? "Пройдено" : "Не пройдено";
    
    public bool IsVisible { get; set; }
}