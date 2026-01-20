using course_work.Data;
using course_work.Models.Classes;
using course_work.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseTests;

public class CourseServiceTests
{
    private async Task<ApplicationDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // уникальная база для каждого теста
                .Options;

            var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        //Тест проверяет создание курса и привязку автора
        [Fact]
        public async Task CreateCourse_ShouldAddCourseAndAuthor()
        {
            var context = await GetInMemoryDbContext();
            var user = new User { Login = "author", Email = "a@example.com", UserTypeId = 2 };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var service = new CourseService(context);
            var course = new Course { Title = "Test Course" };

            var createdCourse = await service.CreateCourse(course, user);

            var dbCourse = await context.Courses
                .Include(c => c.Authors)
                .FirstOrDefaultAsync(c => c.Id == createdCourse.Id);

            dbCourse.Should().NotBeNull();
            dbCourse.Authors.Count.Should().Be(1);
            dbCourse.Authors.First().UserId.Should().Be(user.Id);
        }

        //Тест проверяет получение всех курсов
        [Fact]
        public async Task GetAllCourses_ShouldReturnAllCourses()
        {
            var context = await GetInMemoryDbContext();
            context.Courses.Add(new Course { Title = "Course1" });
            context.Courses.Add(new Course { Title = "Course2" });
            await context.SaveChangesAsync();

            var service = new CourseService(context);
            var courses = await service.GetAllCourses();

            courses.Should().HaveCount(2);
        }

        //Тест проверяет получение курса по Id
        [Fact]
        public async Task GetCourseById_ShouldReturnCorrectCourse()
        {
            var context = await GetInMemoryDbContext();
            var course = new Course { Title = "Course1" };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var service = new CourseService(context);
            var dbCourse = await service.GetCourseById(course.Id);

            dbCourse.Should().NotBeNull();
            dbCourse.Id.Should().Be(course.Id);
        }

        //Тест проверяет получение всех модулей курса
        [Fact]
        public async Task GetAllModules_ShouldReturnModulesForCourse()
        {
            var context = await GetInMemoryDbContext();
            var course = new Course { Title = "Course1" };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            context.Modules.Add(new Module { Title = "Module1", CourseId = course.Id });
            context.Modules.Add(new Module { Title = "Module2", CourseId = course.Id });
            await context.SaveChangesAsync();

            var service = new CourseService(context);
            var modules = await service.GetAllModules(course.Id);

            modules.Should().HaveCount(2);
            modules.Select(m => m.Title).Should().Contain("Module1").And.Contain("Module2");
        }

        //Тест проверяет поиск курсов по названию
        [Fact]
        public async Task SearchCoursesByTitle_ShouldReturnMatchingCourses()
        {
            var context = await GetInMemoryDbContext();
            context.Courses.Add(new Course { Title = "Math" });
            context.Courses.Add(new Course { Title = "Physics" });
            context.Courses.Add(new Course { Title = "History" });
            await context.SaveChangesAsync();

            var service = new CourseService(context);
            var results = await service.SearchCoursesByTitle("phy");

            results.Should().HaveCount(1);
            results.First().Title.Should().Be("Physics");
        }

        //Тест проверяет добавление записи о прохождении курса студентом
        [Fact]
        public async Task TrackCourse_ShouldAddEntryToCourseStudents()
        {
            var context = await GetInMemoryDbContext();
            var user = new User { Login = "student", Email = "s@example.com", UserTypeId = 1 };
            var course = new Course { Title = "Course1" };
            context.Users.Add(user);
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var service = new CourseService(context);
            await service.TrackCourse(course, user);

            var entry = await context.CourseStudents
                .FirstOrDefaultAsync(cs => cs.UserId == user.Id && cs.CourseId == course.Id);

            entry.Should().NotBeNull();
            entry.ProgressPercent.Should().Be(0);
            entry.Completed.Should().BeFalse();
        }

        //Тест проверяет проверку авторства курса
        [Fact]
        public async Task IsAuthorOfCourse_ShouldReturnTrueIfAuthor()
        {
            var context = await GetInMemoryDbContext();
            var user = new User { Login = "author", Email = "a@example.com", UserTypeId = 2 };
            var course = new Course { Title = "Course1" };
            context.Users.Add(user);
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            context.CourseAuthors.Add(new CourseAuthors { CourseId = course.Id, UserId = user.Id });
            await context.SaveChangesAsync();

            var service = new CourseService(context);
            var result = await service.IsAuthorOfCourse(course.Id, user.Id);

            result.Should().BeTrue();
        }

        //Тест проверяет, что неавтор возвращает false
        [Fact]
        public async Task IsAuthorOfCourse_ShouldReturnFalseIfNotAuthor()
        {
            var context = await GetInMemoryDbContext();
            var user = new User { Login = "user", Email = "u@example.com", UserTypeId = 1 };
            var course = new Course { Title = "Course1" };
            context.Users.Add(user);
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            var service = new CourseService(context);
            var result = await service.IsAuthorOfCourse(course.Id, user.Id);

            result.Should().BeFalse();
        }
}