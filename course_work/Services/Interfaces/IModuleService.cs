using System.Collections.Generic;
using System.Threading.Tasks;
using course_work.Models;
using course_work.Models.Classes;

namespace course_work.Services;

public interface IModuleService
{
    Task<ICollection<Module>> GetModules();
    Task<Module> GetModuleById(int id);
    Task AddModule(Module module);
    Task UpdateModule(Module module);
    Task DeleteModule(int id);
    Task<List<LessonPrewie>> GetLessons(int moduleId,int userId,bool author);
}