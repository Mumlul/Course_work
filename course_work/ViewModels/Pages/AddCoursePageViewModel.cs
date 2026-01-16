using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;

namespace course_work.ViewModels.Pages;

public partial class AddCoursePageViewModel:PageViewModelBase
{
    private readonly ICourseService _courseService;
    private readonly Action<Course> _openCourse;
    [ObservableProperty] private User _currentUser;
    [ObservableProperty] private Course _newCourse=new Course();

    public override async Task OnNavigatedTo()
    {
        Console.WriteLine("asdad");
    }

    public AddCoursePageViewModel(ICourseService courseService,User currentUser,Action<Course> openCourse)
    {
        Title = "Add Ciurse";
        _courseService = courseService;
        CurrentUser = currentUser;
        _openCourse=openCourse;
    }
    
    [RelayCommand]
    public async Task CreateCourse()
    {
        await _courseService.CreateCourse(NewCourse,CurrentUser);
        _openCourse?.Invoke(NewCourse);
       
    }
    
    [RelayCommand]
    public async Task SelectPhoto()
    {
        var file = await ChooseFile();
        if (file != null) NewCourse.PreviewImage=await UploadImage(file);
    }
    
}