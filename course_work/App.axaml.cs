using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Text.Json;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using course_work.Data;
using course_work.Extensions;
using course_work.Models.Enums;
using course_work.Services.Service;
using course_work.ViewModels;
using course_work.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace course_work;

public partial class App : Application
{
    private readonly SettingsService _settingsService = new SettingsService();
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        SetTheme(_settingsService.Settings.Theme);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.RemoveAt(0);

        var services = new ServiceCollection();
        services.AddCommonService();

        var provider = services.BuildServiceProvider();
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
    
    
    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
    
    public void SetTheme(AppTheme theme)
    {
        var dictionaries = Resources.MergedDictionaries;
        dictionaries.Clear();

        string path = theme switch
        {
            AppTheme.Light => "Styles/Colors/LightTheme.axaml",
            AppTheme.DarkBlue => "Styles/Colors/DarkBlueTheme.axaml",
            AppTheme.DarkGraphite => "Styles/Colors/DarkGraphiteTheme.axaml",
            _ => throw new ArgumentOutOfRangeException()
        };

        dictionaries.Add(new ResourceInclude(new Uri("avares://course_work/"))
        {
            Source = new Uri($"avares://course_work/{path}")
        });

        RequestedThemeVariant = theme == AppTheme.Light
            ? ThemeVariant.Light
            : ThemeVariant.Dark;

        _settingsService.Settings.Theme = theme;
        _settingsService.Save();
    }
}