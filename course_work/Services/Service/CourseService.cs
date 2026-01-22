using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using course_work.Data;
using course_work.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace course_work.Services;

public class CourseService : ICourseService
{
    private readonly ApplicationDbContext _context;

    public CourseService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<Course>> GetAllCourses()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<Course?> GetCourseById(int id)
    {
       return await  _context.Courses.FindAsync(id); 
    }

    public Task<Course> GetCourseByName(string name)
    {
        throw new System.NotImplementedException();
    }

    public async Task<Course> CreateCourse(Course course, User user)
    {
        course.CreatedAt = DateTime.UtcNow;
        course.UpdatedAt = DateTime.UtcNow;
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        var courseAuthor = new CourseAuthors
        {
            CourseId = course.Id,
            UserId = user.Id
        };
        _context.CourseAuthors.Add(courseAuthor);
        await _context.SaveChangesAsync();

        return course;
    }

    public Task<Course> UpdateCourse(Course course)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<Module>> GetAllModules(int courseId)
    {
        /*var course = await _context.Courses
            .Include(c => c.Modules)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        Console.WriteLine("Bigbob:"+ course.Modules.Count);
        
        var modules = course?.Modules;*/
        
        return _context.Modules
            .Where(m => m.CourseId == courseId)
            .ToListAsync();;
    }

    public Task DeleteCourse(int id)
    {
        throw new System.NotImplementedException();
    }
    
    public async Task<List<Course>> SearchCoursesByTitle(string query, int maxResults = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<Course>();

        var lowerQuery = query.ToLowerInvariant();

        return await _context.Courses
            .Where(c => c.Title.ToLower().Contains(lowerQuery))
            .OrderBy(c => c.Title)
            .Take(maxResults)
            .ToListAsync();
    }

    public async Task TrackCourse(Course course,User user)
    {
        var exists = await _context.CourseStudents
            .AnyAsync(cs => cs.CourseId == course.Id && cs.UserId == user.Id);
        
        if (exists)
            return; 
        
        var courseStudent = new CourseStudents()
        {
            CourseId = course.Id,
            UserId = user.Id,
            ProgressPercent = 0,
            Completed = false,
            StartedAt = DateTime.UtcNow
        };

        _context.CourseStudents.Add(courseStudent);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsAuthorOfCourse(int courseId, int authorId)
    {
        return await _context.CourseAuthors
            .AnyAsync(ca => ca.CourseId == courseId && ca.UserId == authorId);
    }

    public async Task<bool> IsTrackedCourse(int courseId, int userId)
    {
        return await _context.CourseStudents.AnyAsync(cs => cs.CourseId == courseId && cs.UserId == userId);
    }
}