using System;
using System.Collections.Generic;
using System.IO;
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

    public async Task<string> GetLessonFileName(int lessonId)
    {
        var lesson = await _context.Lessons
            .Include(l => l.Module)
            .ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(l => l.Id == lessonId);

        if (lesson == null)
            throw new Exception($"Lesson with id {lessonId} not found");

        if (lesson.Module == null)
            throw new Exception("Module not found");

        if (lesson.Module.Course == null)
            throw new Exception("Course not found");

        var safeTitle = string.Concat(
            lesson.Title.Where(c => !Path.GetInvalidFileNameChars().Contains(c))
        );

        return $"Урок {lessonId}_Course{lesson.Module.Course.Id}_Module{lesson.Module.Id}.docx";
    }

    public async Task CompleteLesson(int lessonId,int userId)
    {
        var progress = await _context.LessonProgresses
            .FirstOrDefaultAsync(lp => lp.LessonId == lessonId && lp.UserId == userId);
        if (progress == null)
        {
            await _context.LessonProgresses.AddAsync(new LessonProgress
            {
                LessonId = lessonId,
                UserId = userId,
                Completed = true,
                CompletedAt = DateTime.Now
            });
        }
        else
        {
            progress.Completed = true;
            progress.CompletedAt = DateTime.Now;
            _context.LessonProgresses.Update(progress);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsCompleteLesson(int lessonId, int userId)
    {
        var progress = await _context.LessonProgresses
            .FirstOrDefaultAsync(lp => lp.LessonId == lessonId && lp.UserId == userId);
        return progress != null && progress.Completed;
    }
    
}