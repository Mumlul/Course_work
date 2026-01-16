using course_work.Data;
using course_work.Services;
using course_work.Services.Interfaces;
using course_work.Services.Service;
using course_work.ViewModels;
using course_work.ViewModels.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace course_work.Extensions;

public static class ServiceCollectionExtentions
{
    public static void AddCommonService(this IServiceCollection services)
    {
        //Register Db Context
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(
                DbConfig.ConnectionString,
                DbConfig.ServerVersion
            );
        });
        
        //Register View Models
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<RegisterPageViewModel>();
        services.AddTransient<LoginPageViewModel>();
        services.AddTransient<MainPageViewModel>();
        services.AddTransient<CoursePageViewModel>();
        services.AddTransient<UserProfilePageViewModel>();
        services.AddTransient<CoursePageViewModel>();
        services.AddTransient<CourseListPageViewModel>();
        services.AddTransient<AddCoursePageViewModel>();
        
        //Register services
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IUserTypeService, UserTypeService>();
        services.AddSingleton<ICourseService, CourseService>();
        services.AddSingleton<IModuleService, ModuleService>();
        services.AddSingleton<ILessonService, LessonService>();
        services.AddSingleton<IUserProfile, UserProfile>();
    }
}