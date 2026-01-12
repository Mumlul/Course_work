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

    public Task<Course> CreateCourse(Course course)
    {
        throw new System.NotImplementedException();
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
}