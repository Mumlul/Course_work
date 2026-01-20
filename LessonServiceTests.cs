using course_work.Data;
using course_work.Models.Classes;
using course_work.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseTests;

public class LessonServiceTests
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

    //Тест проверяет создание урока
    [Fact]
    public async Task CreateLesson_ShouldAddLesson()
    {
        var context = await GetInMemoryDbContext();
        var service = new LessonService(context);

        var lesson = new Lesson { Title = "Lesson1", ModuleId = 1 };
        await service.CreateLesson(lesson);

        var dbLesson = await context.Lessons.FirstOrDefaultAsync();
        dbLesson.Should().NotBeNull();
        dbLesson.Title.Should().Be("Lesson1");
        dbLesson.ModuleId.Should().Be(1);
    }

    //Тест проверяет обновление урока
    [Fact]
    public async Task UpdateLesson_ShouldUpdateLesson()
    {
        var context = await GetInMemoryDbContext();
        var lesson = new Lesson { Title = "OldLesson", ModuleId = 1 };
        context.Lessons.Add(lesson);
        await context.SaveChangesAsync();

        var service = new LessonService(context);
        lesson.Title = "UpdatedLesson";
        await service.UpdateLesson(lesson);

        var dbLesson = await context.Lessons.FirstOrDefaultAsync();
        
        dbLesson!.Title.Should().Be("UpdatedLesson");
    }

    //Тест проверяет получение курса урока
    [Fact]
    public async Task GetCourse_ShouldReturnCourseOfLesson()
    {
        var context = await GetInMemoryDbContext();

        var course = new Course { Title = "Course1" };
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        var module = new Module { Title = "Module1", CourseId = course.Id };
        context.Modules.Add(module);
        await context.SaveChangesAsync();

        var lesson = new Lesson { Title = "Lesson1", ModuleId = module.Id };
        context.Lessons.Add(lesson);
        await context.SaveChangesAsync();

        var service = new LessonService(context);
        var dbCourse = await service.GetCourse(lesson.Id);

        dbCourse.Should().NotBeNull();
        dbCourse.Id.Should().Be(course.Id);
    }

    //Тест проверяет проверку авторства урока
    [Fact]
    public async Task GetAuthor_ShouldReturnTrueIfUserIsAuthor()
    {
        var context = await GetInMemoryDbContext();

        var user = new User { Login = "author", UserTypeId = 2 };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var course = new Course { Title = "Course1" };
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        context.CourseAuthors.Add(new CourseAuthors { CourseId = course.Id, UserId = user.Id });
        await context.SaveChangesAsync();

        var module = new Module { Title = "Module1", CourseId = course.Id };
        context.Modules.Add(module);
        await context.SaveChangesAsync();

        var lesson = new Lesson { Title = "Lesson1", ModuleId = module.Id };
        context.Lessons.Add(lesson);
        await context.SaveChangesAsync();

        var service = new LessonService(context);
        var isAuthor = await service.GetAuthor(lesson.Id, user.Id);

        isAuthor.Should().BeTrue();
    }

    //Тест проверяет проверку авторства урока для неавтора
    [Fact]
    public async Task GetAuthor_ShouldReturnFalseIfUserIsNotAuthor()
    {
        var context = await GetInMemoryDbContext();

        var user = new User { Login = "user", UserTypeId = 1 };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var course = new Course { Title = "Course1" };
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        var module = new Module { Title = "Module1", CourseId = course.Id };
        context.Modules.Add(module);
        await context.SaveChangesAsync();

        var lesson = new Lesson { Title = "Lesson1", ModuleId = module.Id };
        context.Lessons.Add(lesson);
        await context.SaveChangesAsync();

        var service = new LessonService(context);
        var isAuthor = await service.GetAuthor(lesson.Id, user.Id);

        isAuthor.Should().BeFalse();
    }

    //Тест проверяет генерацию имени файла урока
    [Fact]
    public async Task GetLessonFileName_ShouldReturnCorrectFileName()
    {
        var context = await GetInMemoryDbContext();

        var course = new Course { Title = "Course1" };
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        var module = new Module { Title = "Module1", CourseId = course.Id };
        context.Modules.Add(module);
        await context.SaveChangesAsync();

        var lesson = new Lesson { Id = 1, Title = "Lesson:1?*", ModuleId = module.Id };
        context.Lessons.Add(lesson);
        await context.SaveChangesAsync();

        var service = new LessonService(context);
        var fileName = await service.GetLessonFileName(lesson.Id);

        fileName.Should().Be($"Урок {lesson.Id}_Course{course.Id}_Module{module.Id}.docx");
    }

    //Тест проверяет выброс исключения при отсутствии урока
    [Fact]
    public async Task GetLessonFileName_ShouldThrowIfLessonNotFound()
    {
        var context = await GetInMemoryDbContext();
        var service = new LessonService(context);

        Func<Task> act = async () => await service.GetLessonFileName(999);
        await act.Should().ThrowAsync<Exception>().WithMessage("Lesson with id 999 not found");
    }
    
}