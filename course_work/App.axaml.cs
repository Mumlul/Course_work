using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Text.Json;
using Avalonia.Markup.Xaml;
using course_work.Data;
using course_work.Extensions;
using course_work.ViewModels;
using course_work.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace course_work;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.RemoveAt(0);

        var services = new ServiceCollection();
        services.AddCommonService();

        var provider = services.BuildServiceProvider();
        
        /*using (var scope = provider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();
        }*/
        
        
        var vm = provider.GetRequiredService<MainWindowViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }
        
        base.OnFrameworkInitializationCompleted();
    }
    
    
    /*public async override void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.RemoveAt(0);

        var collection = new ServiceCollection();
        collection.AddCommonService();
        
        var service=collection.BuildServiceProvider();

        /*using (var scope = service.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();
        }#1#
        var vm=service.GetRequiredService<MainWindowViewModel>();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow()
            {
                DataContext = vm,
            };
            
            var connectionString =
                "server=185.247.17.245;" +
                "port=3306;" +
                "database=default_db;" +
                "user=gen_user;" +
                "password=&u3q,,T50oQKzX;" +
                "charset=utf8mb3;" +
                "SslMode=Preferred";
            
            using var context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4, 4)))
                    .Options
            );

            bool canConnect = context.Database.CanConnect();
            Console.WriteLine($"Can connect: {canConnect}");
        }

        base.OnFrameworkInitializationCompleted();
    }*/

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}