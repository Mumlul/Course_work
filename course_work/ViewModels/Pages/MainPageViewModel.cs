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
using course_work.Services.Interfaces;
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
    [ObservableProperty] private bool _popup_visible = false;
    [ObservableProperty] private bool _isAuthor = false;
    
    private Action _navigatelogin;
    
    partial void OnCurrentpagemainChanged(PageViewModelBase value)
    {
        if (value != null)
            _ = value.OnNavigatedTo();
    }
    [ObservableProperty] private bool _isopensidebar = true;
    [ObservableProperty] private User _user;
    public ObservableCollection<PageViewModelBase> Pages { get; }
    public MainPageViewModel(IServiceProvider provider,IUserService userService,User currentUser,Action navigatelogin)
    {
        _provider = provider;
        _userService = userService;
        User =currentUser;
        _courseService = provider.GetRequiredService<ICourseService>();

        _navigatelogin = navigatelogin;
        
        Title = "Главная";

        Pages = new ObservableCollection<PageViewModelBase>
        {
            new UserProfilePageViewModel(_userService,provider.GetRequiredService<IUserProfile>(), course => OpenCurse(course),User),
            new CatalogPageViewModel(_userService,_courseService,course => OpenCurse(course),user => OpenProfile(user)),
            new SettingPageViewModel(),
            new CourseListPageViewModel(_courseService,course => OpenCurse(course)),
            /*new AddCoursePageViewModel(_courseService,User,course => OpenCurse(course)),*/
            new TestPageViewModel(_provider.GetRequiredService<ITestService>(),1),
            new CreateTestPageViewModel(_provider.GetRequiredService<ITestService>())
            /*new LessonPageViewModel()*/
            
        };
        
        Currentpagemain = Pages[0];

        if (User.UserTypeId == 2) IsAuthor = true;
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
    
    [RelayCommand]
    public void Open() => Popup_visible = !Popup_visible;
    
    
    [RelayCommand]
    public void NavigateProfile()
    {
        OpenProfile(User);
    }
    
    [RelayCommand]
    public void LogOut()
    {
        _userService.CurrentUser = null;
        _navigatelogin?.Invoke();
    }

    [RelayCommand]
    public void NavAddCurse() => CreateCurse();
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public void OpenCurse(Course course)
    {
        Currentpagemain = null;
        
        var courseVm = ActivatorUtilities.CreateInstance<CoursePageViewModel>(
            _provider,
            _provider.GetRequiredService<ICourseService>(),
            _provider.GetRequiredService<IModuleService>(),
            _provider.GetRequiredService<ILessonService>(),
            (Lesson lesson) => OpenLesson(lesson),
            course,
            User
        );

        if(courseVm == null)
            throw new Exception("Не удалось создать CoursePageViewModel через DI");

        Currentpagemain = courseVm;
    }
    public void OpenProfile(User user)
    {
        Currentpagemain = null;
        
        var profileVm = ActivatorUtilities.CreateInstance<UserProfilePageViewModel>(
            _provider,
            _provider.GetRequiredService<IUserService>(),
            _provider.GetRequiredService<IUserProfile>(),
            (Course course) => OpenCurse(course),
            user
        );
        
        Currentpagemain = profileVm;
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

    public void CreateCurse()
    {
        Currentpagemain = null;

        var createvm = ActivatorUtilities.CreateInstance<AddCoursePageViewModel>(
            _provider,
            _provider.GetRequiredService<ICourseService>(),
            User,
            (Course course) => OpenCurse(course)
        );
        
        Currentpagemain = createvm;

    }
    
}