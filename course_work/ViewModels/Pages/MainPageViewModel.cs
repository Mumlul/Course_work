using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels.Pages;

public class MainPageViewModel:PageViewModelBase, IReactiveObject
{
    public ObservableCollection<PageViewModelBase> Pages { get; } = new()
    {
        new UserProfilePageViewModel(),
        new CourseListPageViewModel()
    };
    [Reactive] public PageViewModelBase CurrentPage { get; set; }
    
    public ReactiveCommand<PageViewModelBase, Unit> NavigateCommand { get; }
    public MainPageViewModel(MainWindowViewModel? root = null)
    {
        Title = "Главная";
        CurrentPage = Pages[0];
    }

    public void RaisePropertyChanging(PropertyChangingEventArgs args)
    {
        throw new NotImplementedException();
    }

    public void RaisePropertyChanged(PropertyChangedEventArgs args)
    {
        throw new NotImplementedException();
    }
}