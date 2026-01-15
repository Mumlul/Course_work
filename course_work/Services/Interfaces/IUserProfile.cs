using System.Threading.Tasks;
using course_work.Models.Classes;

namespace course_work.Services.Interfaces;

public interface IUserProfile
{
    UserProfile Profile { get; set; }
    
    Task<UserProfile> LoadProfileAsync(int  userId);
    Task UpdateProfileAsync(UserProfile profile);
    Task DeleteProfileAsync(UserProfile profile);
}