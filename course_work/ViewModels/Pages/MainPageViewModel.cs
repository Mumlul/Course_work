using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels.Pages;

public partial class MainPageViewModel:PageViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentpagemain;
    
    [ObservableProperty] private bool _isopensidebar = true;
    public ObservableCollection<PageViewModelBase> Pages { get; } = new()
    {
        new UserProfilePageViewModel(),
        new CourseListPageViewModel()
    };
    
    public MainPageViewModel()
    {
        Title = "Главная";
        Currentpagemain=Pages[0];
    }
    
    //Возможно пригодится что бы не писать 
    /*public ObservableCollection<MenuItem> test { get; } = new ObservableCollection<MenuItem>()
        { 
            new MenuItem
            {
                Title = "Search",
                SourceImg = "../../Assets/icons/search.svg"
            },
            new MenuItem
            {
                Title = "Profile",
                SourceImg = "../../Assets/icons/arrow-left-square.svg"
            },
            new MenuItem
            {
                Title = "Course List",
                SourceImg = "../../Assets/icons/course_list.svg"
            },
            new MenuItem
            {
                Title = "Settings",
                SourceImg = "../../Assets/icons/settings.svg"
            }
        };*/

    [RelayCommand]
    private void OpenPane()
    {
        Isopensidebar = !Isopensidebar;
        foreach (var page in Pages)
        {
            page.TextVisible = !page.TextVisible;
        }
    }

}