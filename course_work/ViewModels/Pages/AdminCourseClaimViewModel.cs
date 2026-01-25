using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;

namespace course_work.ViewModels.Pages;

public partial class AdminCourseClaimViewModel:PageViewModelBase
{
    private readonly ICourseService _courseService;
    private Action<Course> _openCourse;
    private Action<User> _openUser;
    
    [ObservableProperty] private CourseComplaint _selectedComplaint;
    [ObservableProperty] private string _userMessage;
    [ObservableProperty] private string _authorMessage;
    [ObservableProperty] private bool _openWindow;
    [ObservableProperty] private int _fixDays;
    
    public ObservableCollection<CourseComplaint> Complaints { get; set; } = new();
    
    public AdminCourseClaimViewModel(ICourseService courseService,
        Action<Course> openCourse, 
        Action<User> openUser)
    {
        Title = "Жалобы на курсы";
        Image = "../../Assets/icons/file-01.svg";
        _courseService = courseService;
        _openCourse = openCourse;
        _openUser = openUser;
    }

    public override async Task OnNavigatedTo()
    {
        var claims = await _courseService.GetAllComplaints();
        
        Complaints.Clear();
        foreach (var claim in claims)
            Complaints.Add(claim);
    }

    partial void OnSelectedComplaintChanged(CourseComplaint value)
    {
        OpenWindow=true;
    }


    [RelayCommand]
    public async Task OpenCourse()
    {
        _openCourse?.Invoke(SelectedComplaint.Course);
    } 

    [RelayCommand]
    public async Task OpenUser() => _openUser?.Invoke(SelectedComplaint.User);

    [RelayCommand]
    public async Task SendMessageToUser()
    {
        
    }

    [RelayCommand]
    public async Task SendMessageToAuthor()
    {
        
    }

    [RelayCommand]
    public void CloseWindow()
    {
        OpenWindow=false;
        SelectedComplaint = null;
    }


}