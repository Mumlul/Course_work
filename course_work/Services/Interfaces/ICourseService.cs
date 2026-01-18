using System.Collections.Generic;
using System.Threading.Tasks;
using course_work.Models.Classes;

namespace course_work.Services;

public interface ICourseService
{
    
    Task<ICollection<Course>> GetAllCourses();
    Task<Course> GetCourseById(int id);
    Task<Course> GetCourseByName(string name);
    Task<Course> CreateCourse(Course course, User user);
    Task<Course> UpdateCourse(Course course);
    Task<List<Module>> GetAllModules(int courseId);
    Task DeleteCourse(int id);
    Task<List<Course>> SearchCoursesByTitle(string query, int maxResults = 10);
    Task TrackCourse(Course course,User user);
    
    Task<bool> IsAuthorOfCourse(int courseId, int authorId);
}