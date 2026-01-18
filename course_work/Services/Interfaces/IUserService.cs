using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using course_work.Models.Classes;

namespace course_work.Services;

public interface IUserService
{
    User CurrentUser { get; set; }
    UserProfile Profile { get; set; }
    Task<ICollection<User>> GetAllUsers();
    Task<User> GetUserById(int id);
    Task<User> GetUserByUsername(string username);
    Task<User> GetUserByEmail(string email);
    Task DeleteUserById(User user);
    Task<User> AddUser(User user);
    Task UpdateUser(User user);
    Task<bool> CheckPassword(string login, string password);
    Task<ObservableCollection<Course>> GetAllCourses(User user);
    Task<UserProfile> GetUserProfile(User user);
    Task<List<User>> GetAllAuthors();
    Task<List<Course>> GetAithorsCurse(int userId);
    
}