using System.Reactive;
using ReactiveUI;

namespace course_work.ViewModels.Pages;

public class LoginPageViewModel:PageViewModelBase
{
    private readonly MainWindowViewModel _root;

    public ReactiveCommand<Unit, Unit> LoginCommand { get; }
    public ReactiveCommand<Unit, Unit> GoToRegisterCommand { get; }

    public LoginPageViewModel(MainWindowViewModel root)
    {
        _root = root;
        Title = "Вход";

        LoginCommand = ReactiveCommand.Create(() =>
        {
            _root.ShowViewModel(new MainPageViewModel(_root)); 
        });

        GoToRegisterCommand = ReactiveCommand.Create(() =>
            _root.ShowViewModel(new RegisterPageViewModel(_root)));
    }
}