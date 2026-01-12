using System.Collections.Generic;
using System.Threading.Tasks;
using course_work.Models.Classes;

namespace course_work.Services;

public class LessonService:ILessonService
{
    public Task<ICollection<Lesson>> GetLessons()
    {
        throw new System.NotImplementedException();
    }

    public Task<Lesson> GetLesson(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<Lesson> CreateLesson(Lesson lesson)
    {
        throw new System.NotImplementedException();
    }

    public Task<Lesson> UpdateLesson(Lesson lesson)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteLesson(int id)
    {
        throw new System.NotImplementedException();
    }
}