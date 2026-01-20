using course_work.Data;
using course_work.Models.Classes;
using course_work.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseTests;

public class TestUserService
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

    //Тест проверяет добавление пользователя и автоматическое создание профиля
    [Fact]
    public async Task AddUser_ShouldCreateUserWithProfile()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);

        var user = new User { Login = "testuser", Email = "test@example.com", UserTypeId = 1 };
        var createdUser = await service.AddUser(user, "Password123");

        createdUser.Id.Should().BeGreaterThan(0);
        createdUser.Profile.Should().NotBeNull();
        createdUser.Login.Should().Be("testuser");
    }

    //Тест проверяет, что метод CheckPassword возвращает true для правильного пароля
    [Fact]
    public async Task CheckPassword_ShouldReturnTrueForCorrectPassword()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);

        var user = new User { Login = "user1", Email = "u1@example.com", UserTypeId = 1 };
        await service.AddUser(user, "Password123");

        var result = await service.CheckPassword("user1", "Password123");

        result.Should().BeTrue();
    }

    //Тест проверяет, что метод CheckPassword возвращает false для неправильного пароля
    [Fact]
    public async Task CheckPassword_ShouldReturnFalseForWrongPassword()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);

        var user = new User { Login = "user2", Email = "u2@example.com", UserTypeId = 1 };
        await service.AddUser(user, "Password123");

        var result = await service.CheckPassword("user2", "WrongPassword");

        result.Should().BeFalse();
    }

    //Тест проверяет получение всех пользователей
    [Fact]
    public async Task GetAllUsers_ShouldReturnAllUsers()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);

        await service.AddUser(new User { Login = "u1", Email = "a@example.com", UserTypeId = 1 }, "p1");
        await service.AddUser(new User { Login = "u2", Email = "b@example.com", UserTypeId = 1 }, "p2");

        var users = await service.GetAllUsers();

        users.Should().HaveCount(2);
    }

    //Тест проверяет получение пользователя по Id
    [Fact]
    public async Task GetUserById_ShouldReturnCorrectUser()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);
        var createdUser = await service.AddUser(new User { Login = "u1", Email = "a@example.com", UserTypeId = 1 }, "p1");

        var user = await service.GetUserById(createdUser.Id);

        user.Should().NotBeNull();
        user.Id.Should().Be(createdUser.Id);
    }

    //Тест проверяет получение пользователя по имени
    [Fact]
    public async Task GetUserByUsername_ShouldReturnCorrectUser()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);
        await service.AddUser(new User { Login = "u2", Email = "b@example.com", UserTypeId = 1 }, "p2");

        var user = await service.GetUserByUsername("u2");

        user.Should().NotBeNull();
        user.Login.Should().Be("u2");
    }

    //Тест проверяет получение пользователя по email
    [Fact]
    public async Task GetUserByEmail_ShouldReturnCorrectUser()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);
        await service.AddUser(new User { Login = "u3", Email = "c@example.com", UserTypeId = 1 }, "p3");

        var user = await service.GetUserByEmail("c@example.com");

        user.Should().NotBeNull();
        user.Email.Should().Be("c@example.com");
    }

    //Тест проверяет удаление пользователя и его профиля
    [Fact]
    public async Task DeleteUserById_ShouldRemoveUserAndProfile()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);
        var user = await service.AddUser(new User { Login = "deluser", Email = "del@example.com", UserTypeId = 1 }, "pass");

        await service.DeleteUserById(user);

        var dbUser = await context.Users.FindAsync(user.Id);
        var dbProfile = await context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);

        dbUser.Should().BeNull();
        dbProfile.Should().BeNull();
    }

    //Тест проверяет обновление данных пользователя и профиля
    [Fact]
    public async Task UpdateUser_ShouldUpdateUserAndProfile()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);

        var user = await service.AddUser(new User { Login = "upuser", Email = "up@example.com", UserTypeId = 1 }, "pass");
        user.Email = "new@example.com";
        user.Profile.Description = "New Description";

        await service.UpdateUser(user);

        var dbUser = await context.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == user.Id);
        dbUser.Email.Should().Be("new@example.com");
        dbUser.Profile.Description.Should().Be("New Description");
    }

    //Тест проверяет получение всех курсов пользователя
    [Fact]
    public async Task GetAllCourses_ShouldReturnUserCourses()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);

        var user = await service.AddUser(new User { Login = "stud", Email = "stud@example.com", UserTypeId = 1 }, "pass");
        var course = new Course { Title = "Course1" };
        context.Courses.Add(course);
        await context.SaveChangesAsync();
        context.CourseStudents.Add(new CourseStudents { UserId = user.Id, CourseId = course.Id });
        await context.SaveChangesAsync();

        var courses = await service.GetAllCourses(user);

        courses.Should().HaveCount(1);
        courses.First().Title.Should().Be("Course1");
    }

    //Тест проверяет получение профиля пользователя
    [Fact]
    public async Task GetUserProfile_ShouldReturnProfile()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);
        var user = await service.AddUser(new User { Login = "profuser", Email = "p@example.com", UserTypeId = 1 }, "pass");

        var profile = await service.GetUserProfile(user);

        profile.Should().NotBeNull();
        profile.UserId.Should().Be(user.Id);
    }

    //Тест проверяет получение всех авторов
    [Fact]
    public async Task GetAllAuthors_ShouldReturnOnlyAuthors()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);
        await service.AddUser(new User { Login = "author1", Email = "a1@example.com", UserTypeId = 2 }, "pass");
        await service.AddUser(new User { Login = "user1", Email = "u1@example.com", UserTypeId = 1 }, "pass");

        var authors = await service.GetAllAuthors();

        authors.Should().HaveCount(1);
        authors.First().UserTypeId.Should().Be(2);
    }

    //Тест проверяет получение курсов автора
    [Fact]
    public async Task GetAithorsCurse_ShouldReturnAuthorsCourses()
    {
        var context = await GetInMemoryDbContext();
        var service = new UserService(context);

        var author = await service.AddUser(new User { Login = "author2", Email = "a2@example.com", UserTypeId = 2 }, "pass");
        var course = new Course { Title = "AuthorCourse" };
        context.Courses.Add(course);
        await context.SaveChangesAsync();
        context.CourseAuthors.Add(new CourseAuthors { CourseId = course.Id, UserId = author.Id });
        await context.SaveChangesAsync();

        var courses = await service.GetAithorsCurse(author.Id);

        courses.Should().HaveCount(1);
        courses.First().Title.Should().Be("AuthorCourse");
    }

    

}