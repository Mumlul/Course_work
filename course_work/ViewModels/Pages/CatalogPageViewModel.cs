using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models;
using course_work.Models.Classes;
using course_work.Services;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels.Pages;

public partial class CatalogPageViewModel:PageViewModelBase
{
    private readonly IUserService _userService;
    private readonly ICourseService _courseService;

    [ObservableProperty] private string? searcText;
    [ObservableProperty] private User? _selectedAuthor;
    [ObservableProperty] private Course? _selectedCourse;
    [ObservableProperty] private SearchBlock? _searchItem;
    [ObservableProperty] private bool _isDialogOpened=false;
    public ObservableCollection<Course> Courses { get; set; } = new();
    public ObservableCollection<User> Authors { get; } = new();
    public ObservableCollection<SearchBlock> FilteredSearchItems { get; } = new();
    public IEnumerable PreviewCourses => Courses.Take(6);
    public IEnumerable PreviewAuthors => Authors.Take(6);
    public IEnumerable<SearchBlock> SearchItems =>
        Courses.Select(c => new SearchBlock
            {
                Type = SearchType.Course,
                Course = c,
                SeacrchText = c.Title
            })
            .Concat(
                Authors.Select(a => new SearchBlock
                {
                    Type = SearchType.Author,
                    Author = a,
                    SeacrchText = a.Login
                })
            );

    private readonly Action<Course> _openCourse;
    private readonly Action<User> _openProfile;

    public async override Task OnNavigatedTo()
    {
        Console.WriteLine("1");
        if (Courses.Count > 0)
            return;
        
        var courses = await _courseService.GetAllCourses();
        Courses.Clear();
        foreach (var course in courses)
            Courses.Add(course);
        
        OnPropertyChanged(nameof(PreviewCourses));
        
        var authors = await _userService.GetAllAuthors();
        Authors.Clear();
        foreach (var author in authors)
            Authors.Add(author);
        
        OnPropertyChanged(nameof(PreviewAuthors));
        OnPropertyChanged(nameof(SearchItems));
        
        Console.WriteLine($"Courses: {Courses.Count}");
        Console.WriteLine($"PreviewCourses: {PreviewCourses.Cast<Course>().Count()}");
    }
    
    public CatalogPageViewModel(IUserService userService, 
        ICourseService courseService,
        Action<Course> openCourse,
        Action<User?> openUserProfile
        )
    {
        Console.WriteLine("2");
        Title = "Главная";
        Image = "../../Assets/icons/home-04.svg";
        _userService = userService;
        _courseService = courseService;
        _openCourse=openCourse;
        _openProfile=openUserProfile;
    }
    
    partial void OnSearcTextChanged(string? value)
    {
        _ = UpdateSuggestionsAsync(value);
    }

    private async Task UpdateSuggestionsAsync(string? query)
    {
        FilteredSearchItems.Clear();

        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            return;

        var q = query.Trim();

        foreach (var item in SearchItems.Where(x =>
                         x.SeacrchText.Contains(q, StringComparison.OrdinalIgnoreCase))
                     .Take(20))
        {
            FilteredSearchItems.Add(item);
        }
    }

    partial void OnSearchItemChanged(SearchBlock? value)
    {
        if(value != null)
        
        switch (value.Type)
        {
            case  SearchType.Course: 
                SelectedCourse=value.Course;
                IsDialogOpened=true;
                break;
            
            case  SearchType.Author:
                _openProfile?.Invoke(value.Author);
                break;
            
        }
    }

    [RelayCommand]
    public async Task ViewCurse()
    {
        if(SelectedCourse!=null) _openCourse?.Invoke(SelectedCourse);
    }

    [RelayCommand]
    public async Task CloseDialog()
    {
        IsDialogOpened = false;
        SelectedCourse = null;
    }

    partial void OnSelectedAuthorChanged(User? value)
    {
        if(value == null) return;
        _openProfile?.Invoke(value);
    }

    partial void OnSelectedCourseChanged(Course? value)
    {
        if (value != null) IsDialogOpened = true;
    }
}