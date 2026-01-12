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
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels.Pages;

public partial class UserProfilePageViewModel : PageViewModelBase
{
    private readonly IUserService _userService;
    private readonly Action<Course> _openCourse;

    [ObservableProperty] private User _user;
    [ObservableProperty] private Course _selectedCourse;
    public ObservableCollection<Course> Courses { get; } = new();

    [ObservableProperty] private Bitmap _avatar;
    
    //Выгрузка фото
    [RelayCommand]
    private async Task LoadAvatarAsync()
    {
        using var http = new HttpClient();

        using var response = await http.GetAsync(
            "https://6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672.s3.twcstorage.ru/photo.jpg",
            HttpCompletionOption.ResponseHeadersRead
        );

        response.EnsureSuccessStatusCode();

        // КЛЮЧЕВОЕ МЕСТО
        var bytes = await response.Content.ReadAsByteArrayAsync();

        using var ms = new MemoryStream(bytes);
        Avatar = new Bitmap(ms);
    }

    

    public UserProfilePageViewModel(IUserService userService, Action<Course> openCourse)
    {
        _userService = userService;
        User = _userService.CurrentUser;
        _openCourse = openCourse;
        Title = "UserProfile";
        Image = "../../Assets/icons/arrow-left-square.svg";
        _ = LoadCoursesAsync();
        
    }

    public async Task LoadCoursesAsync()
    {
        var courses = await _userService.GetAllCourses(_user);
        Courses.Clear();
        foreach (var course in courses)
            Courses.Add(course);
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
        var config = new AmazonS3Config
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
        Console.WriteLine("Файл загружен!");
    }
}