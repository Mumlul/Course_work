using course_work.Data;
using course_work.Models.Classes;
using course_work.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseTests;

public class ModuleServiceTests
{
    private async Task<ApplicationDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        //Тест проверяет добавление нового модуля
        [Fact]
        public async Task AddModule_ShouldAddModule()
        {
            var context = await GetInMemoryDbContext();
            var service = new ModuleService(context);

            var module = new Module { Title = "Module1", PreviewImage = "preview.png", CourseId = 1 };
            await service.AddModule(module);

            var dbModule = await context.Modules.FirstOrDefaultAsync(m => m.Title == "Module1");
            dbModule.Should().NotBeNull();
            dbModule.PreviewImage.Should().Be("preview.png");
            dbModule.CourseId.Should().Be(1);
        }

        //Тест проверяет обновление существующего модуля
        [Fact]
        public async Task UpdateModule_ShouldUpdateExistingModule()
        {
            var context = await GetInMemoryDbContext();
            var module = new Module { Title = "OldModule", PreviewImage = "old.png", CourseId = 1 };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            var service = new ModuleService(context);
            module.Title = "UpdatedModule";
            module.PreviewImage = "new.png";

            await service.UpdateModule(module);

            var dbModule = await context.Modules.FirstOrDefaultAsync(m => m.Id == module.Id);
            dbModule.Title.Should().Be("UpdatedModule");
            dbModule.PreviewImage.Should().Be("new.png");
        }

        //Тест проверяет выброс исключения при обновлении несуществующего модуля
        [Fact]
        public async Task UpdateModule_ShouldThrowExceptionIfModuleNotFound()
        {
            var context = await GetInMemoryDbContext();
            var service = new ModuleService(context);
            var module = new Module { Id = 999, Title = "NonExistent", PreviewImage = "none.png" };

            Func<Task> act = async () => await service.UpdateModule(module);
            await act.Should().ThrowAsync<Exception>().WithMessage("Module not found");
        }

        //Тест проверяет получение списка уроков модуля
        [Fact]
        public async Task GetLessons_ShouldReturnLessonsForModule()
        {
            var context = await GetInMemoryDbContext();
            var module = new Module { Title = "Module1", CourseId = 1 };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            context.Lessons.Add(new Lesson { Title = "Lesson1", ModuleId = module.Id });
            context.Lessons.Add(new Lesson { Title = "Lesson2", ModuleId = module.Id });
            await context.SaveChangesAsync();

            var service = new ModuleService(context);
            var lessons = await service.GetLessons(module.Id);

            lessons.Should().HaveCount(2);
            lessons.Select(l => l.Title).Should().Contain("Lesson1").And.Contain("Lesson2");
        }
}