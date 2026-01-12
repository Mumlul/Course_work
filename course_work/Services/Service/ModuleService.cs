using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using course_work.Data;
using course_work.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace course_work.Services;

public class ModuleService:IModuleService
{
    private readonly ApplicationDbContext _context;   
    public ModuleService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    public Task<ICollection<Module>> GetModules()
    {
        throw new System.NotImplementedException();
    }

    public Task<Module> GetModuleById(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<Module> AddModule(Module module)
    {
        throw new System.NotImplementedException();
    }

    public Task<Module> UpdateModule(Module module)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteModule(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<Lesson>> GetLessons(int moduleId)
    {
        return _context.Lessons
            .Where(l => l.ModuleId == moduleId)
            .ToListAsync();;
    }
}