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
using course_work.Views.Pages;
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
    
    partial void OnCurrentpagemainChanged(
        PageViewModelBase oldValue,
        PageViewModelBase newValue)
    {
        if (oldValue != null)
            _ = oldValue.OnNavigatedFrom();

        if (newValue != null)
            _ = newValue.OnNavigatedTo();
    }
    [ObservableProperty] private bool _isopensidebar = false;
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


        if (User.UserTypeId != 3)
        {
            Pages = new ObservableCollection<PageViewModelBase>
            {
                new CatalogPageViewModel(_userService,_courseService,course => OpenCurse(course),user => OpenProfile(user)),
                new UserProfilePageViewModel(_userService,provider.GetRequiredService<IUserProfile>(), course => OpenCurse(course),User),
                new SettingPageViewModel(_userService),
                new CourseListPageViewModel(_courseService,course => OpenCurse(course)),
                /*new AddCoursePageViewModel(_courseService,User,course => OpenCurse(course)),*/
                /*new TestPageViewModel(_provider.GetRequiredService<ITestService>(),1),*/
                /*new CreateTestPageViewModel(_provider.GetRequiredService<ITestService>())*/
            };
        }
        else
        {
            Pages = new ObservableCollection<PageViewModelBase>
            {
                new AdminCourseClaimViewModel(_courseService,
                    course => OpenCurse(course),
                    user => OpenProfile(user)),
                new AdminUserClaimViewModel(_userService,user => OpenProfile(user)),
                new SettingPageViewModel(_userService),
            };
        }
        
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
            (int courseId,bool v) => OpenTest(courseId,v),
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
            lesson,
            (Course course) => OpenCurse(course),
            User.Id
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

    
    //Если будет писарь курса то переходим на редактирвоание курса если же обычный обыватьель то путь головой деумает
    public void OpenTest(int courseId,bool av)
    {
        Currentpagemain = null;
        if (av == true)
        {
            var testvm = ActivatorUtilities.CreateInstance<CreateTestPageViewModel>(
                _provider,
                _provider.GetRequiredService<ITestService>(),
                courseId,
                (Course course) => OpenCurse(course)
            );
        
            Currentpagemain = testvm;
        }
        else
        {
            var test=ActivatorUtilities.CreateInstance<TestPageViewModel>(
                _provider,
                _provider.GetRequiredService<ITestService>(),
                courseId,
                User
            );
            
            Currentpagemain = test;
        }
    }
    
}