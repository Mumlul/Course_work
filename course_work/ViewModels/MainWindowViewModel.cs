using System.Collections.ObjectModel;
using System.Reactive;
using course_work.ViewModels.Pages;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [Reactive] public object CurrentViewModel { get; set; }

    public MainWindowViewModel()
    {
        // Стартовая страница — логин
        CurrentViewModel = new LoginPageViewModel(this);
    }

    // Все страницы будут вызывать этот метод для перехода
    public void ShowViewModel(object vm) => CurrentViewModel = vm;
}