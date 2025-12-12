using System.Reactive;
using course_work.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels.Pages;

public class UserProfilePageViewModel:PageViewModelBase
{
    public User_ user { get; set; }

    [Reactive] public PageViewModelBase CurrentPage { get; set; }
    
    public UserProfilePageViewModel()
    {
        Title = "UserProfile";
    }
}