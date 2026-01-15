using System.Threading.Tasks;
using course_work.Data;
using course_work.Services.Interfaces;

namespace course_work.Services.Service;

public class UserProfile:IUserProfile
{
    public Models.Classes.UserProfile Profile { get; set; }
    private readonly ApplicationDbContext _context;

    public UserProfile(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Models.Classes.UserProfile> LoadProfileAsync(int userId)
    {
        return await _context.UserProfiles.FindAsync(userId);
    }

    public async Task UpdateProfileAsync(Models.Classes.UserProfile profile)
    {
        _context.UserProfiles.Update(profile);
        await _context.SaveChangesAsync();
    }

    public Task DeleteProfileAsync(Models.Classes.UserProfile profile)
    {
        _context.UserProfiles.Remove(profile);
        return _context.SaveChangesAsync();
    }
}