using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;
using course_work.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels.Pages;

public partial class UserProfilePageViewModel : PageViewModelBase
{
    private readonly IUserService _userService;
    private readonly IUserProfile _userProfile;
    private readonly Action<Course> _openCourse;

    [ObservableProperty] private User _user;
    [ObservableProperty] private UserProfile _profile;
    [ObservableProperty] private Course _selectedCourse;
    [ObservableProperty] private Bitmap _avatar;
    [ObservableProperty] private bool _isAuthor=false;
    [ObservableProperty] private bool _openClaimButton=false;
    [ObservableProperty] private string _claimMessage = "";
    public ObservableCollection<Course> Courses { get; } = new();
    public ObservableCollection<Course> AllCursesAuthor { get; } = new();
    public ObservableCollection<Course> CompleteCurse { get; } = new();
    public ObservableCollection<TestResult> CompleteTest { get; } = new();
    
    [ObservableProperty] private bool isUser=false;

    public async override Task OnNavigatedTo()
    {
        var courses = await _userService.GetAllCourses(User);
        Courses.Clear();
        
        foreach (var course in courses)
            Courses.Add(course);

        var completed = await _userService.GetCompleteCourses(User.Id);
        
        CompleteCurse.Clear();
        foreach (var course in completed)
            CompleteCurse.Add(course);
        
        if (User.UserTypeId == 2)
        {
            IsAuthor = true;
            AllCursesAuthor.Clear();
            var aithorsc = await _userService.GetAithorsCurse(User.Id);
            foreach (var curse in aithorsc)
                AllCursesAuthor.Add(curse);
        }

        var completedtest = await _userService.GetCompletedTest(User.Id);
        CompleteTest.Clear();
        foreach (var test in completedtest)
            CompleteTest.Add(test);

    }
    //Поработать над загрузками потому что если выбирать из другого метода у примеру просмотр страницы то будет трудно + фото не меняется + кнпока изменения тоже не приятно стоит
    public UserProfilePageViewModel(IUserService userService,IUserProfile userProfile, Action<Course> openCourse,User currentUser)
    {
        _userService = userService;
        _userProfile = userProfile;
        User = currentUser;
        Profile = User.Profile;
        _openCourse = openCourse;
        Title = "UserProfile";
        ImageBlock = "../../Assets/icons/user-profile-03.svg";
        _ = LoadAvatarAsync();
        if (User.Id != _userService.CurrentUser.Id) isUser = false;
        else  isUser = true;
    }
    
    [RelayCommand]
    private async Task ChangeAvatar()
    {
        var file_path = await ChooseFile();
        if (!string.IsNullOrEmpty(file_path))
        {
            Profile.Avatar= await UploadImage(file_path);
            Console.WriteLine($"Image:a {file_path} a");
            await _userProfile.UpdateProfileAsync(Profile);
            await LoadAvatarAsync();
        }
        
    }
    
    private async Task LoadAvatarAsync()
    {
        if (string.IsNullOrEmpty(Profile?.Avatar)) return;

        try
        {
            using var http = new HttpClient();
            var bytes = await http.GetByteArrayAsync(Profile.Avatar);
            using var ms = new MemoryStream(bytes);
            Avatar = new Bitmap(ms);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить аватар: {ex.Message}");
            Avatar = null;
        }
    }
    
    
    [RelayCommand]
    public void CourseView()
    {
        if (SelectedCourse != null)
            _openCourse?.Invoke(SelectedCourse);
    }
    
    partial void OnSelectedCourseChanged(Course value)
    {
        if (value is null)
            return;
        _openCourse?.Invoke(value);
        SelectedCourse = null;
    }

    [RelayCommand]
    public async Task AddClaim()
    {
        var claim = new UserComplaint
        {
            FromUser = _userService.CurrentUser,
            ToUser = User,
            ComplaintText = ClaimMessage,
            CreatedAt = DateTime.UtcNow,
            FromUserId = _userService.CurrentUser.Id,
            ToUserId = User.Id
        };

        try
        {
            await _userService.AddClaim(claim);
            ClaimMessage=string.Empty;
            OpenClaimButton = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    [RelayCommand]
    public void OpenClaim()=>OpenClaimButton=true;

    [RelayCommand]
    public void CloseClaim() => OpenClaimButton = false;


}