using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using course_work.Data;
using course_work.Models.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace course_work.Services;



public class UserService:IUserService
{
    private readonly ApplicationDbContext _context;
    public User CurrentUser { get; set; } = new User();
    public UserProfile Profile { get; set; } = new UserProfile();
    
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    
   public async Task<ICollection<User>> GetAllUsers()
    {
        return await _context.Users
            .Include(u => u.Profile)
            .ToListAsync();
    }

    public async Task<User> GetUserById(int id)
    {
        return await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Login == username);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task DeleteUserById(User user)
    {
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (profile != null)
        {
            _context.UserProfiles.Remove(profile);
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> AddUser(User user, string plainPassword)
    {
        user.Password = _passwordHasher.HashPassword(user, plainPassword);
        var addedUser = _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var profile = new UserProfile
        {
            UserId = addedUser.Entity.Id
        };
        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();
        addedUser.Entity.Profile = profile;
        return addedUser.Entity;
    }

    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);

        if (user.Profile != null)
        {
            _context.UserProfiles.Update(user.Profile);
        }

        await _context.SaveChangesAsync();
    }

   

    public async Task<bool> CheckPassword(string login, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (user == null) return false;

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        return result == PasswordVerificationResult.Success;
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
        return await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == user.Id);
    }

    public Task<List<User>> GetAllAuthors()
    {
        return _context.Users
            .Include(u => u.Profile)
            .Where(u => u.UserTypeId == 2)
            .ToListAsync();
    }

    public async Task<List<Course>> GetAithorsCurse(int userId)
    {
        return await _context.CourseAuthors
            .Where(ca => ca.UserId == userId)
            .Select(ca => ca.Course)
            .ToListAsync();
    }
    
    
    public async Task MigratePlainPasswordsToHashed()
    {
        var users = await _context.Users.ToListAsync();
        var passwordHasher = new PasswordHasher<User>();

        foreach (var user in users)
        {
            if (!string.IsNullOrWhiteSpace(user.Password) && !user.Password.StartsWith("$"))
            {
                user.Password = passwordHasher.HashPassword(user, user.Password);
            }
        }

        _context.Users.UpdateRange(users);
        await _context.SaveChangesAsync();
    }
    
    public async Task<int> GetCourseProgressPercent(int userId, int courseId)
    {
        var progress = await _context.CourseStudents
            .Where(cs => cs.UserId == userId && cs.CourseId == courseId)
            .Select(cs => cs.ProgressPercent)
            .FirstOrDefaultAsync();

        return progress;
    }

    public async Task<List<UserComplaint>> GetAllComplaints()
    {
        return await _context.UserComplaints
            .Include(c => c.FromUser)  
                .ThenInclude(u => u.Profile)
            .Include(c => c.ToUser)
                .ThenInclude(u => u.Profile)
            .OrderByDescending(c => c.CreatedAt) 
            .ToListAsync();
    }

    public async Task<bool> CheckEmail(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<bool> CheckLogin(string login)
    {
        return await _context.Users
            .AnyAsync(u => u.Login == login);
    }

    public async Task AddClaim(UserComplaint uc)
    {
        var exists = await _context.UserComplaints
            .AnyAsync(c => c.FromUserId == uc.FromUserId &&
                           c.ToUserId == uc.ToUserId);
        if (exists)
            throw new InvalidOperationException("Жалоба на этого пользователя уже отправлена");
        await _context.UserComplaints.AddAsync(uc);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Course>> GetCompleteCourses(int userId)
    {
        return await _context.CourseStudents
            .Where(cs => cs.UserId == userId && cs.Completed)
            .Select(cs => cs.Course)
            .ToListAsync();
    }

    public async Task<List<TestResult>> GetCompletedTest(int userId)
    {
        return await _context.TestResults
            .Where(tc => tc.UserId == userId)
            .Include(t=>t.Test)
            .ToListAsync();
    }

    public async Task UpdateProfile(UserProfile profile)
    {
        _context.UserProfiles.Update(profile);
        await _context.SaveChangesAsync();
    }
}