using System.Collections.Generic;
using System.Threading.Tasks;
using course_work.Models.Classes;
using Course = course_work.Models.Course;

namespace course_work.Services;

public interface ICourseService
{
    Task<ICollection<Course>> GetAllCourses();
    Task<Course> GetCourseById(int id);
    Task<Course> GetCourseByName(string name);
    Task<Course> CreateCourse(Course course);
    Task<Course> UpdateCourse(Course course);
    Task<ICollection<Module>> GetAllModules();
    Task DeleteCourse(int id);
}