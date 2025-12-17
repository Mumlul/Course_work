using System.Collections.Generic;
using System.Threading.Tasks;
using course_work.Models.Classes;

namespace course_work.Services;

public interface ILessonService
{
    Task<ICollection<Lesson>> GetLessons();
    Task<Lesson> GetLesson(int id);
    Task<Lesson> CreateLesson(Lesson lesson);
    Task<Lesson> UpdateLesson(Lesson lesson);
    Task DeleteLesson(int id);
}