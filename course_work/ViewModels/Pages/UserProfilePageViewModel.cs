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
    public ObservableCollection<Course> Courses { get; } = new();
    public ObservableCollection<Course> AllCursesAuthor { get; } = new();
    
    [ObservableProperty] private bool isUser=false;

    public async override Task OnNavigatedTo()
    {
        var courses = await _userService.GetAllCourses(User);
        Courses.Clear();
        foreach (var course in courses)
            Courses.Add(course);
        Console.WriteLine($"USER ser:{_userService.CurrentUser.Id}\n USER :{User.Id}");
        
        
        Console.WriteLine($"BOOL:{IsUser}");
        
        if (User.UserTypeId == 2)
        {
            IsAuthor = true;
            AllCursesAuthor.Clear();
            var aithorsc = await _userService.GetAithorsCurse(User.Id);
            foreach (var curse in aithorsc)
                AllCursesAuthor.Add(curse);
        }
        else
        {
            AllCursesAuthor.Clear();
            foreach (var c in Courses.Take(6))
                AllCursesAuthor.Add(c);
        }
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
        Image = "../../Assets/icons/user-profile-03.svg";
        _ = LoadAvatarAsync();
        if (User.Id != _userService.CurrentUser.Id) isUser = false;
        else  isUser = true;
    }
    
    [RelayCommand]
    private async Task ChangeAvatar()
    {
        var file_path = await ChooseFile();
        Profile.Avatar= await UploadImage(file_path);
        await _userProfile.UpdateProfileAsync(Profile);
        Console.WriteLine(Profile.Avatar);
        await LoadAvatarAsync();
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
    
    //Загрузка фото
    [RelayCommand]
    public async Task Test()
    {
      
       /* var config = new AmazonS3Config
        {
            ServiceURL = "https://s3.twcstorage.ru", 
            ForcePathStyle = true 
        };
        
        using var client = new AmazonS3Client("2H4NLFXQSWUC8A31U1PB", "EYBr2GBUGTtSdS7fTM8XgBXwSEUDROFMK1wpCwcF", config);

        var putRequest = new PutObjectRequest
        {
            BucketName = "6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672",
            Key = "photo.jpg", 
            FilePath = @"C:\Users\mumlul\Downloads\коала.jpg",
            ContentType = "image/jpeg"
        };

        var response = await client.PutObjectAsync(putRequest);
        Console.WriteLine("Файл загружен!");*/
    }
}