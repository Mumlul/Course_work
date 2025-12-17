using System.Threading.Tasks;
using course_work.Models.Classes;

namespace course_work.Services;

public interface IUserTypeService
{
    Task<UserType> AddUserType(UserType userType);
}