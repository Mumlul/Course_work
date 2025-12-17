using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using course_work.Data;
using course_work.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace course_work.Services;

public class UserService:IUserService
{
    private readonly ApplicationDbContext _context;
    

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserById(int id)
    {
        return await  _context.Users.FindAsync(id); 
    }

    public async Task<User> GetUserByUsername(string username)
    {
       return await  _context.Users.FindAsync(username);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await  _context.Users.FindAsync(email);
    }

    public async Task DeleteUserById(User user)
    {
       _context.Users.Remove(user);
       await _context.SaveChangesAsync();
    }
    
    public async Task<User> AddUser(User user)
    {
        var c=_context.Users.Add(user);
        await _context.SaveChangesAsync();
        return c.Entity;
    }

    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await  _context.SaveChangesAsync();
    }

    public async Task<bool> CheckPassword(User user, string password)
    {
        var _user = await _context.Users
            .FirstOrDefaultAsync(u => u.Login == user.Login);
        
        if (_user == null) return false;
        
        return user.Password == password;
        
    }
}