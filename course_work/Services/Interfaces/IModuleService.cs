using System.Collections.Generic;
using System.Threading.Tasks;
using course_work.Models.Classes;

namespace course_work.Services;

public interface IModuleService
{
    Task<ICollection<Module>> GetModules();
    Task<Module> GetModuleById(int id);
    Task<Module> AddModule(Module module);
    Task<Module> UpdateModule(Module module);
    Task DeleteModule(int id);
    Task<ICollection<Lesson>> GetLessons(Module module);
}