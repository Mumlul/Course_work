using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using course_work.Data;
using course_work.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace course_work.Services;

public class UserService:IUserService
{
    private readonly ApplicationDbContext _context;
    public User CurrentUser { get; set; } = new User();
    public UserProfile Profile { get; set; } = new UserProfile();

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
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Login == username);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
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
        
        var profile = new UserProfile
        {
            UserId = c.Entity.Id
        };
        
        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();
        
        c.Entity.Profile = profile;
        
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

    public async Task<ObservableCollection<Course>> GetAllCourses(User user)
    {
        var courses = await _context.CourseStudents
            .Where(cs => cs.UserId == user.Id)
            .Select(cs => cs.Course)
            .ToListAsync();
        
        return new ObservableCollection<Course>(courses);
    }

    public async Task<UserProfile> GetUserProfile(User user)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == user.Id);
        return profile;
    }

    public Task<List<User>> GetAllAuthors()
    {
        return _context.Users.Where(u => u.UserTypeId == 2).ToListAsync();
    }
}