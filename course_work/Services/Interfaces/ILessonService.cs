using System.Collections.Generic;
using System.Threading.Tasks;
using course_work.Models.Classes;

namespace course_work.Services;

public interface ILessonService
{
    Task<ICollection<Lesson>> GetLessons();
    Task<Lesson> GetLesson(int id);
    Task CreateLesson(Lesson lesson);
    Task UpdateLesson(Lesson lesson);
    Task DeleteLesson(int id);
    Task<Course> GetCourse(int id);
    Task<bool> GetAuthor(int id, int userId);
    Task<string> GetLessonFileName(int lessonId);
    Task CompleteLesson(int lessonId,int userId);
    Task<bool> IsCompleteLesson(int lessonId, int userId);
    Task UpdateCourseProgress(int courseId, int userId);
    Task<int> GetCurseId(int lessonId);
}