using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;

namespace course_work.ViewModels.Pages;

public partial class CoursePageViewModel:PageViewModelBase
{
    private ICourseService _courseService;
    private IModuleService _moduleService;
    private ILessonService _lessonService;
    
    private readonly Action<Lesson> _openLesson;
    public ObservableCollection<Module> Module { get; } = new();
    public ObservableCollection<Lesson> Lessons { get; } = new();
    [ObservableProperty] private Course _currentcourse;
    [ObservableProperty] private int _test;
    [ObservableProperty] private bool _isDialogOpen = false;
    [ObservableProperty] private Module _selectedModule;
    [ObservableProperty] private Lesson _selectedLesson;
    [ObservableProperty] private User _currentUser;
    
    [ObservableProperty] private Bitmap _image;
    [ObservableProperty] private bool _isAuthor=false;
    [ObservableProperty] private Bitmap _moduleImage;


    async partial void OnSelectedModuleChanged(Module? value)
    {
        if (value is null)
            return;
        OpenDialog();
        ModuleImage = await ConvertImageToByteArray(value.PreviewImage);
        Console.WriteLine(value.PreviewImage);
    }

    partial void OnSelectedLessonChanged(Lesson? value)
    {
        if (value is null)
            return;
        _openLesson?.Invoke(value);
    }

    public override async Task OnNavigatedTo()
    {
        var modules = await _courseService.GetAllModules(Currentcourse.Id);
        Module.Clear();
        foreach (var module in modules)
            Module.Add(module);
        
        if (string.IsNullOrEmpty(Currentcourse?.PreviewImage)) return;

        try
        {
            using var http = new HttpClient();
            var bytes = await http.GetByteArrayAsync(Currentcourse?.PreviewImage);
            using var ms = new MemoryStream(bytes);
            Image = new Bitmap(ms);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить аватар: {ex.Message}");
            Image = null;
        }

        IsAuthor= await _courseService.IsAuthorOfCourse(Currentcourse.Id,CurrentUser.Id);
    }


    public CoursePageViewModel(
    ICourseService courseService,
    IModuleService moduleService,
    ILessonService lessonService,
    Action<Lesson> openLesson,
    Course course,
    User user)
    {
        Title = "Course Page";
        _courseService = courseService;
        _moduleService= moduleService;
        _lessonService= lessonService;
        Currentcourse = course;
        _openLesson=openLesson;
        CurrentUser = user;
    }
    
    [RelayCommand]
    public void CloseDialog()
    {
        IsDialogOpen = false;
        
        SelectedModule = null; 
        Lessons.Clear();
    }

    [RelayCommand]
    public async Task OpenDialog()
    {
        var lessons = await _moduleService.GetLessons(SelectedModule.Id);
        Lessons.Clear();
        foreach (var lesson in lessons)
            Lessons.Add(lesson);
        IsDialogOpen = true;
    }

    [RelayCommand]
    public async Task TrackCourse()
    {
        await _courseService.TrackCourse(Currentcourse, CurrentUser);
    }

    [RelayCommand]
    public async Task AddModule()
    {
        var module = new Module()
        {
            CourseId = Currentcourse.Id,
            Title = $"Модуль {Module.Count+1}",
            OrderIndex = Module.Count+1
        };
        
        _moduleService.AddModule(module);
        Module.Add(module);
        SelectedModule = module;
        IsDialogOpen = true;
    }

    [RelayCommand]
    public async Task AddLesson()
    {
        var lesson = new Lesson()
        {
            ModuleId = SelectedModule.Id,
            Title = $"Урок {Lessons.Count + 1}",
            OrderIndex = Lessons.Count + 1,
            Slug = $"Урок {Lessons.Count + 1}",
            ContentJson = "{}",
            LessonType = LessonType.Text,
            CreatedAt = DateTime.Today,
            UpdatedAt = DateTime.Today,
        };

        await _lessonService.CreateLesson(lesson);
        Lessons.Add(lesson);
        /*SelectedLesson = lesson;*/
    }

    [RelayCommand]
    public async Task DeleteModule()
    {
        
    }

    [RelayCommand]
    public void Close()
    {
        SelectedModule = null;
        IsDialogOpen = false;
    }

    [RelayCommand]
    public async Task ChangeImage()
    {
        var file = await ChooseFile();
        if (file is null) return;
        var url=await UploadImage(file);
        ModuleImage = await ConvertImageToByteArray(url);
        SelectedModule.PreviewImage = url;
        _moduleService.UpdateModule(SelectedModule);
    }
    
}