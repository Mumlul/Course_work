using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls.Chrome;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;

namespace course_work.ViewModels.Pages;

public partial class CourseListPageViewModel:PageViewModelBase
{
    private readonly ICourseService _courseService;
    private readonly Action<Course> _openCourse;
    [ObservableProperty] private bool _isDialogOpen;
    [ObservableProperty] private Course _newCourse;
    [ObservableProperty] private Course _selectedCourse;
    [ObservableProperty] private string _searchText;
    public ObservableCollection<Course> Courses { get; } = new();
    partial void OnSelectedCourseChanged(Course value) =>IsDialogOpen = true;
    
   
    public override async Task OnNavigatedTo()
    {
        Courses.Clear();
        var courses = await _courseService.GetAllCourses();
        foreach (var course in courses)
            Courses.Add(course);
        Console.WriteLine("ASd:"+Courses.Count);
    }
    public CourseListPageViewModel(ICourseService _cs,Action<Course> openCourse)
    {
        Title = "Course List";
        Image = "../../Assets/icons/file-01.svg";
        _courseService = _cs;
        _openCourse = openCourse;
    }
    [RelayCommand] public void CloseDialog() => IsDialogOpen = false;
    [RelayCommand] public async Task ViewCurse()
    {
        _openCourse?.Invoke(_selectedCourse);
    }
    
    
    
    
}