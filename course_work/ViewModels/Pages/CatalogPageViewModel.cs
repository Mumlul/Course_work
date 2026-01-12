using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels.Pages;

public partial class CatalogPageViewModel:PageViewModelBase
{
    private readonly IUserService _userService;
    private readonly ICourseService _courseService;

    [ObservableProperty] private string? searcText;
    
    public ObservableCollection<Course> Courses { get; set; } = new();
    public ObservableCollection<User> Authors { get; } = new();
    public ObservableCollection<Module> Modules { get; set; } = new();
    public ObservableCollection<Course> SuggestedCourses { get; } = new();
    [ObservableProperty] private Course? _selectedCourse;


    public async override Task OnNavigatedTo()
    {
        if (Courses.Count > 0)
            return;
        
        var courses = await _courseService.GetAllCourses();

        Courses.Clear();
        foreach (var course in courses.Take(6))
            Courses.Add(course);
        Console.WriteLine(Courses.Count);
        
    }
    
    public CatalogPageViewModel(IUserService userService, ICourseService courseService)
    {
        Title = "Главная";
        _userService = userService;
        _courseService = courseService;
    }
    
    partial void OnSearcTextChanged(string? value)
    {
        _ = UpdateSuggestionsAsync(value);
    }

    private async Task UpdateSuggestionsAsync(string? query)
    {
        SuggestedCourses.Clear();

        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            return;

        try
        {
            var matches = await _courseService.SearchCoursesByTitle(query.Trim(), maxResults: 12);

            foreach (var course in matches)
            {
                SuggestedCourses.Add(course);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка поиска: {ex.Message}");
        }
    }
    
}