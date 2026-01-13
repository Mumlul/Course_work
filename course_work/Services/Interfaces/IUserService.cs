using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using course_work.Models.Classes;

namespace course_work.Services;

public interface IUserService
{
    User CurrentUser { get; set; }

    Task<ICollection<User>> GetAllUsers();
    Task<User> GetUserById(int id);
    Task<User> GetUserByUsername(string username);
    Task<User> GetUserByEmail(string email);
    Task DeleteUserById(User user);
    Task<User> AddUser(User user);
    Task UpdateUser(User user);
    Task<bool> CheckPassword(User user, string password);
    Task<ObservableCollection<Course>> GetAllCourses(User user);
    Task<UserProfile> GetUserProfile(User user);
}