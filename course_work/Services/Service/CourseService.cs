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

    public async Task UpdateCourse(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }

    public Task<List<Module>> GetAllModules(int courseId)
    {
        return _context.Modules
            .Where(m => m.CourseId == courseId)
            .ToListAsync();;
    }

    public async Task DeleteCourse(int id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
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
    
    public async Task<int> GetCourseProgressPercent(int userId, int courseId)
    {
        var progress = await _context.CourseStudents
            .Where(cs => cs.UserId == userId && cs.CourseId == courseId)
            .Select(cs => cs.ProgressPercent)
            .FirstOrDefaultAsync();

        return progress;
    }

    public async Task StartTrackCourse(int userId, int courseId)
    {
        var tc = new CourseStudents()
        {
            CourseId = courseId,
            UserId = userId,
            ProgressPercent = 0,
            StartedAt = DateTime.UtcNow,
            Completed = false
        };
        Console.WriteLine("отслеживается");
        _context.CourseStudents.Add(tc);
        await _context.SaveChangesAsync();
    }

    public async Task EndTrackCourse(int userId, int courseId)
    {
        var tc= _context.CourseStudents.FirstOrDefault(cs => cs.CourseId == courseId && cs.UserId == userId);
        
        _context.CourseStudents.Remove(tc);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsComplete(int userId, int courseId)
    {
        var progress = await _context.CourseStudents
            .Where(cs => cs.UserId == userId && cs.CourseId == courseId)
            .Select(cs => cs.ProgressPercent)
            .FirstOrDefaultAsync();
        
        return progress>0?true:false;
    }

    public async Task AddReview(CourseReview review)
    {
        var exists = await _context.CourseReviews
            .AnyAsync(r =>
                r.CourseId == review.CourseId &&
                r.UserId == review.UserId);

        if (exists)
            throw new InvalidOperationException("Пользователь уже оставил отзыв на этот курс");

        review.CreatedAt = DateTime.UtcNow;

        _context.CourseReviews.Add(review);
        await _context.SaveChangesAsync();
    }

    public async Task AddClaim(CourseComplaint complaint)
    {
        var exists = await _context.CourseComplaints
            .AnyAsync(c =>
                c.CourseId == complaint.CourseId &&
                c.UserId == complaint.UserId);

        if (exists)
            throw new InvalidOperationException("Жалоба на этот курс уже отправлена");

        complaint.CreatedAt = DateTime.UtcNow;

        _context.CourseComplaints.Add(complaint);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CourseReview>> LastReview(int courseId)
    {
        return await _context.CourseReviews
            .AsNoTracking()
            .Where(r => r.CourseId == courseId)
            .OrderByDescending(r => r.CreatedAt)
            .Include(r => r.User)
            .ThenInclude(u => u.Profile)
            .Take(3)
            .ToListAsync();
    }

    public async Task<double> GetAverageRating(int courseId)
    {
        var ratings = await _context.CourseReviews
            .AsNoTracking()
            .Where(r => r.CourseId == courseId)
            .Select(r => r.Rating)
            .ToListAsync();

        if (ratings.Count == 0)
            return 0;

        return Math.Round(ratings.Average(r => r), 1);
    }

    public async Task<List<CourseComplaint>> GetAllComplaints()
    {
        return await _context.CourseComplaints
            .Include(c => c.User)
                .ThenInclude(u => u.Profile)
            .Include(c => c.Course) 
            .OrderByDescending(c => c.CreatedAt) 
            .ToListAsync();
    }

    public async Task<User> GetCourseAuthor(int courseId)
    {
        var author = await _context.CourseAuthors
            .Where(ca => ca.CourseId == courseId)
            .Select(ca => ca.User)
            .FirstOrDefaultAsync();

        if (author == null)
            throw new InvalidOperationException("Автор курса не найден");

        return author;
    }
}