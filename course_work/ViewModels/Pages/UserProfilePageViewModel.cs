using System;
using System.Collections.ObjectModel;
using System.IO;
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

    public async override Task OnNavigatedTo()
    {
        var courses = await _userService.GetAllCourses(User);
        Courses.Clear();
        foreach (var course in courses)
            Courses.Add(course);
    }
    //Поработать над загрузками потому что если выбирать из другого метода у примеру просмотр страницы то будет трудно + фото не меняется + кнпока изменения тоже не приятно стоит
    public UserProfilePageViewModel(IUserService userService,IUserProfile userProfile, Action<Course> openCourse,User currentUser)
    {
        _userService = userService;
        _userProfile = userProfile;
        User = _userService.CurrentUser;
        if (currentUser != null) User = currentUser;
        Profile = _userService.Profile;
        _openCourse = openCourse;
        Title = "UserProfile";
        Image = "../../Assets/icons/arrow-left-square.svg";
        /*_ = LoadCoursesAsync();*/
        _ = LoadAvatarAsync();
    }

    /*public async Task LoadCoursesAsync()
    {
        var courses = await _userService.GetAllCourses(User);
        Courses.Clear();
        foreach (var course in courses)
            Courses.Add(course);
    }*/
    
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