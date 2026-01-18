using System.Collections.Generic;
using System.Threading.Tasks;
using course_work.Data;
using course_work.Models.Classes;

namespace course_work.Services;

public class LessonService:ILessonService
{
    private readonly ApplicationDbContext _context;

    public LessonService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<ICollection<Lesson>> GetLessons()
    {
        throw new System.NotImplementedException();
    }

    public Task<Lesson> GetLesson(int id)
    {
        throw new System.NotImplementedException();
    }

    public async Task CreateLesson(Lesson lesson)
    {
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
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