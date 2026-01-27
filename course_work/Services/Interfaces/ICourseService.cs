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
    Task UpdateCourse(Course course);
    Task<List<Module>> GetAllModules(int courseId);
    Task DeleteCourse(int id);
    Task<List<Course>> SearchCoursesByTitle(string query, int maxResults = 10);
    Task TrackCourse(Course course,User user);
    Task<bool> IsAuthorOfCourse(int courseId, int authorId);
    Task<bool> IsTrackedCourse(int courseId, int userId);
    Task<int> GetCourseProgressPercent(int userId, int courseId);
    Task StartTrackCourse(int userId, int courseId);
    Task EndTrackCourse(int userId, int courseId);
    Task<bool> IsComplete(int userId, int courseId);
    Task AddReview(CourseReview cr);
    Task AddClaim(CourseComplaint cc);
    Task<List<CourseReview>> LastReview (int courseId);
    Task<double>  GetAverageRating(int courseId);
    Task<List<CourseComplaint>> GetAllComplaints();
    Task<User> GetCourseAuthor(int courseId);
    


}