using System.Threading.Tasks;
using course_work.Data;
using course_work.Models.Classes;

namespace course_work.Services;

public class UserTypeService : IUserTypeService
{
    
    private readonly ApplicationDbContext _context;

    public UserTypeService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<UserType> AddUserType(UserType userType)
    {
        var c=_context.UserTypes.Add(userType);
        await _context.SaveChangesAsync();
        return c.Entity;
    }
}