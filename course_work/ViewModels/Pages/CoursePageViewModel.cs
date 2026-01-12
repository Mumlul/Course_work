using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;

namespace course_work.ViewModels.Pages;

public partial class CoursePageViewModel:PageViewModelBase
{
    private ICourseService _courseService;
    private IModuleService _moduleService;
    private readonly Action<Lesson> _openLesson;
    public ObservableCollection<Module> Module { get; } = new();
    public ObservableCollection<Lesson> Lessons { get; } = new();
    [ObservableProperty] private Course _currentcourse;
    [ObservableProperty] private int _test;
    [ObservableProperty] private bool _isDialogOpen = false;
    [ObservableProperty] private Module _selectedModule;
    [ObservableProperty] private Lesson _selectedLesson;
    
    partial void OnSelectedModuleChanged(Module? value)
    {
        if (value is null)
            return;
        OpenDialog();
    }

    partial void OnSelectedLessonChanged(Lesson? value)
    {
        if (value is null)
            return;
        _openLesson?.Invoke(value);
    }
    
    
    public CoursePageViewModel(ICourseService courseService,
    IModuleService moduleService, 
    Action<Lesson> openLesson,
    Course course)
    {
        Title = "Course Page";
        Image = "../../Assets/icons/arrow-left-square.svg";
        _courseService = courseService;
        _moduleService= moduleService;
        Currentcourse = course;
        _openLesson=openLesson;
        LoadModules(Currentcourse.Id);
    }

    public async Task LoadModules(int id)
    {
        var modules = await _courseService.GetAllModules(Currentcourse.Id);
        Module.Clear();
        foreach (var module in modules)
            Module.Add(module);
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


}