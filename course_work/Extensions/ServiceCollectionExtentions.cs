using course_work.Data;
using course_work.Services;
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
        
        
        //Register services
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserTypeService, UserTypeService>();
    }
}