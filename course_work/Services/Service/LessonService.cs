using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using course_work.Data;
using course_work.Models.Classes;
using Microsoft.EntityFrameworkCore;

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

    public async Task UpdateLesson(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
    }

    public Task DeleteLesson(int id)
    {
        throw new System.NotImplementedException();
    }

    public async Task<Course> GetCourse(int id)
    {
        var lesson = await _context.Lessons
            .Include(l => l.Module)
            .ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(l => l.Id == id);

        return lesson?.Module.Course;
    }

    public async Task<bool> GetAuthor(int id,int userId)
    {
        return await _context.Lessons
            .Where(l => l.Id == id)
            .Select(l => l.Module.Course.Id)
            .Join(
                _context.CourseAuthors,
                courseId => courseId,
                ca => ca.CourseId,
                (courseId, ca) => ca.UserId
            )
            .AnyAsync(uId => uId == userId);
    }
}