using System.Collections.ObjectModel;
using System.Reactive;
using CommunityToolkit.Mvvm.ComponentModel;
using course_work.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels.Pages;

public class UserProfilePageViewModel:PageViewModelBase
{
   

    [Reactive] public PageViewModelBase CurrentPage { get; set; }
    
    public UserProfilePageViewModel()
    {
        Title = "UserProfile";
        Image = "../../Assets/icons/arrow-left-square.svg";
        
    }
    
    // Сюда будет передоваться пользователь получается у него с помощью этого мы получим его курсы а пока это будет заменять моя колллекция

    public ObservableCollection<Course> Courses { get; } = new ObservableCollection<Course>()
    {
        new Course()
        {
            Author = "Victor", Title = "Газосварзик",
            Description = "Краткая истори о том как попасть в лапы злому гоблину"
        },
        new Course()
        {
            Author = "Victor1", Title = "Газосварзик",
            Description = "Краткая истори о том как попасть в лапы злому гоблину"
        },
        new Course()
        {
            Author = "Victor2", Title = "Газосварзик",
            Description = "Краткая истори о том как попасть в лапы злому гоблину"
        },
        new Course()
        {
            Author = "Victor", Title = "Газосварзик",
            Description = "Краткая истори о том как попасть в лапы злому гоблину"
        },
        
    };
}