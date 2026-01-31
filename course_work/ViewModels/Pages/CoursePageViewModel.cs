using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models;
using course_work.Models.Classes;
using course_work.Services;

namespace course_work.ViewModels.Pages;

public partial class CoursePageViewModel:PageViewModelBase
{
    private ICourseService _courseService;
    private IModuleService _moduleService;
    private ILessonService _lessonService;
    
    private readonly Action<Lesson> _openLesson;
    private readonly Action<int,User,bool> _openTest;
    private readonly Action<User> _openUser;
     
    public ObservableCollection<Module> Module { get; } = new();
    public ObservableCollection<LessonPrewie> Lessons { get; } = new();
    [ObservableProperty] private Course _currentcourse;
    [ObservableProperty] private int _test;
    [ObservableProperty] private bool _isDialogOpen = false;
    [ObservableProperty] private Module _selectedModule;
    [ObservableProperty] private LessonPrewie _selectedLesson;
    [ObservableProperty] private User _currentUser;
    
    [ObservableProperty] private string _image;
    [ObservableProperty] private bool _isAuthor=false;
    [ObservableProperty] private Bitmap _moduleImage;
    [ObservableProperty] private bool _isTracked;
    [ObservableProperty] private int _completePercent;
    [ObservableProperty] private bool _isComplete;
    
    
    [ObservableProperty] private string _reviewMessage;
    [ObservableProperty] private int _reviewRating;
    [ObservableProperty] private bool _reviewWindow = false;
    
    [ObservableProperty] private string _claimMessage;
    [ObservableProperty] private bool _claimWindow = false;

    public ObservableCollection<CourseReview> LastReview { get; set; } = new();
    [ObservableProperty] private double _rating;


    async partial void OnSelectedModuleChanged(Module? value)
    {
        if (value is null)
            return;
        ModuleImage = await ConvertImageToByteArray(value.PreviewImage);
        Console.WriteLine(value.PreviewImage);
        OpenDialog();

        
    }

    partial void OnSelectedLessonChanged(LessonPrewie? value)
    {
        if (value is null)
            return;
        _openLesson?.Invoke(value.Lesson);
    }
    
    //Сделать MessageBox при удаление отслеживания что ващ текущий прогресс будет утерян в случае того что вы отменяете отслеживание
    partial void OnIsTrackedChanged(bool value)
    {
        Console.WriteLine(value);
    }

    public override async Task OnNavigatedTo()
    {
        var modules = await _courseService.GetAllModules(Currentcourse.Id);
        Module.Clear();
        foreach (var module in modules)
            Module.Add(module);

        if (string.IsNullOrEmpty(Currentcourse?.PreviewImage))
            return;
        /*Currentcourse.PreviewImage = @"C:\Users\st310-07\Documents\Плоских\f\course_work\Assets\test\";*/

        Image = Currentcourse?.PreviewImage;

        IsAuthor = await _courseService.IsAuthorOfCourse(Currentcourse.Id,CurrentUser.Id);
        IsTracked = await _courseService.IsTrackedCourse(Currentcourse.Id, CurrentUser.Id);

        if (IsTracked)
        {
            CompletePercent = await _courseService.GetCourseProgressPercent(CurrentUser.Id, Currentcourse.Id);
            IsComplete = await _courseService.IsComplete(CurrentUser.Id, Currentcourse.Id);
        }

        LastReview.Clear();
        var reviews = await _courseService.LastReview(Currentcourse.Id);
        foreach (var review in reviews)
            LastReview.Add(review);

        Rating = await _courseService.GetAverageRating(Currentcourse.Id);
    }

    public override async Task OnNavigatedFrom()
    {
        await _courseService.UpdateCourse(Currentcourse);
    }


    public CoursePageViewModel(
    ICourseService courseService,
    IModuleService moduleService,
    ILessonService lessonService,
    Action<Lesson> openLesson,
    Action<int,User,bool> openTest,
    Action<User> openUser,
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
        _openTest = openTest;
        _openUser = openUser;
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
        var lessons = await _moduleService.GetLessons(SelectedModule.Id,CurrentUser.Id,IsAuthor);
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
            ContentUrl = "",
            LessonType = LessonType.Text,
            CreatedAt = DateTime.Today,
            UpdatedAt = DateTime.Today,
        };

        await _lessonService.CreateLesson(lesson);
        
        Lessons.Add(new LessonPrewie
        {
            Lesson = lesson,
            IsCompleted = false,
        });
    }

    [RelayCommand]
    public async Task DeleteModule()
    {
        await _moduleService.DeleteModule(SelectedModule.Id);
        SelectedModule = null;
        
        Module.Clear();
        var modules = await _courseService.GetAllModules(Currentcourse.Id);
        foreach (var module in modules)
            Module.Add(module);
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

        Console.WriteLine(ModuleImage);
        /*var file = await ChooseFile();
        if (file is null) return;
        var url=await UploadImage(file);
        ModuleImage = await ConvertImageToByteArray(url);
        SelectedModule.PreviewImage = url;
        _moduleService.UpdateModule(SelectedModule);*/
    }

    [RelayCommand]
    public async Task NavigateToCreateTest()
    {
        _openTest?.Invoke(Currentcourse.Id,null,true);
        
    }


    [RelayCommand]
    public async Task ChangeStatus()
    {
        /*Console.WriteLine("ChangeStatus");*/
        if (IsTracked != true)
           _courseService.EndTrackCourse(CurrentUser.Id, Currentcourse.Id);
           /*Console.WriteLine("Delete");*/
        else
           _courseService.StartTrackCourse(CurrentUser.Id, Currentcourse.Id);
           /*Console.WriteLine("Create");*/
        
    }

    [RelayCommand]
    public async Task NavigateToCompleteTets()
    {
        _openTest?.Invoke(Currentcourse.Id,CurrentUser,false);
    }

    [RelayCommand]
    public async Task AddReview()
    {
        if (ReviewRating < 1 || ReviewRating > 5)
            return;

        if (string.IsNullOrWhiteSpace(ReviewMessage))
            return;

        var review = new CourseReview
        {
            CourseId = Currentcourse.Id,
            UserId = CurrentUser.Id,
            Rating = (byte)ReviewRating,
            ReviewText = ReviewMessage,
            IsApproved = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        try
        {
            await _courseService.AddReview(review);

            ReviewMessage = string.Empty;
            ReviewRating = 0;
            ReviewWindow = false;
            
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    [RelayCommand]
    public async Task AddClaim()
    {
        if (string.IsNullOrWhiteSpace(ClaimMessage))
            return;

        var claim = new CourseComplaint
        {
            CourseId = Currentcourse.Id,
            UserId = CurrentUser.Id,
            ComplaintText = ClaimMessage,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await _courseService.AddClaim(claim);

            ClaimMessage = string.Empty;
            ClaimWindow = false;
            
            LastReview.Clear();
            var reviews = await _courseService.LastReview(Currentcourse.Id);
            foreach (var review in reviews)
                LastReview.Add(review);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    [RelayCommand]
    public void OpenReview() => ReviewWindow = true;
    
    [RelayCommand]
    public void CloseReview()=> ReviewWindow = false;
    
    [RelayCommand] 
    public void OpenClaim()=> ClaimWindow = true;
    
    [RelayCommand]
    public void CloseClaim()=> ClaimWindow = false;

    [RelayCommand]
    public async Task ChangeCourseImage()
    {
        var file = await ChooseFile();
        if (file is null||file=="") return;
        Image= await UploadImage(file);
        Currentcourse.PreviewImage = Image;
        _courseService.UpdateCourse(Currentcourse);
    }

    [RelayCommand]
    public async Task DeleteCourse()
    {
        await _courseService.DeleteCourse(Currentcourse.Id);
        _openUser?.Invoke(CurrentUser);
    }
    
    
    


}