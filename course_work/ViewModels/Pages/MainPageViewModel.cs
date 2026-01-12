using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models;
using course_work.Models.Classes;
using course_work.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels.Pages;

public partial class MainPageViewModel:PageViewModelBase
{
    private readonly IServiceProvider _provider;
    private readonly IUserService _userService;
    private readonly ICourseService _courseService;

    [ObservableProperty] private PageViewModelBase _currentpagemain;
    
    partial void OnCurrentpagemainChanged(PageViewModelBase value)
    {
        if (value != null)
            _ = value.OnNavigatedTo();
    }
    
    [ObservableProperty] private bool _isopensidebar = true;
    [ObservableProperty] private User _user;

    public ObservableCollection<PageViewModelBase> Pages { get; }

    public MainPageViewModel(IServiceProvider provider,IUserService userService)
    {
        _provider = provider;
        _userService = userService;
        User = _userService.CurrentUser;
        _courseService = provider.GetRequiredService<ICourseService>();

        Title = "Главная";

        Pages = new ObservableCollection<PageViewModelBase>
        {
            new UserProfilePageViewModel(_userService, course => OpenCurse(course)),
            new CatalogPageViewModel(_userService,_courseService),
            new SettingPageViewModel(),
            /*new LessonPageViewModel()*/
        };
        
        Currentpagemain = Pages[0];
    }
    
    [RelayCommand]
    private void OpenPane()
    {
        Isopensidebar = !Isopensidebar;
        foreach (var page in Pages)
        {
            page.TextVisible = !page.TextVisible;
        }
    }

    public void OpenCurse(Course course)
    {
        Currentpagemain = null;
        
        var courseVm = ActivatorUtilities.CreateInstance<CoursePageViewModel>(
            _provider,
            _provider.GetRequiredService<ICourseService>(),
            _provider.GetRequiredService<IModuleService>(),
            (Lesson lesson) => OpenLesson(lesson),
            course
        );

        if(courseVm == null)
            throw new Exception("Не удалось создать CoursePageViewModel через DI");

        Currentpagemain = courseVm;
    }

    public void OpenLesson(Lesson lesson)
    {
        
        Currentpagemain = null;
        var lessonVm = ActivatorUtilities.CreateInstance<LessonPageViewModel>(
            _provider,
            _provider.GetRequiredService<ILessonService>(),
            lesson
        );
        
        if(lessonVm == null)
            throw new Exception("Не удалось создать CoursePageViewModel через DI");
        
        Currentpagemain = lessonVm;
    }
    

}