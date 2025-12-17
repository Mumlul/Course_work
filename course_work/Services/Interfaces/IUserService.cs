using System.Collections.Generic;
using System.Threading.Tasks;
using course_work.Models.Classes;

namespace course_work.Services;

public interface IUserService
{
    Task<ICollection<User>> GetAllUsers();
    Task<User> GetUserById(int id);
    Task<User> GetUserByUsername(string username);
    Task<User> GetUserByEmail(string email);
    Task DeleteUserById(User user);
    Task<User> AddUser(User user);
    Task UpdateUser(User user);
    Task<bool> CheckPassword(User user, string password);
}