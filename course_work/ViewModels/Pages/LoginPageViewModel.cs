using System;
using System.Reactive;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models;
using course_work.Models.Classes;
using course_work.Services;
using ReactiveUI;

namespace course_work.ViewModels.Pages;

public partial class LoginPageViewModel:PageViewModelBase
{
    private readonly MainWindowViewModel _mainWindowVm;
    private readonly IUserService _userService;
    private readonly Action OnLoginSuccess;
    private readonly Action Register;
    
    [ObservableProperty] 
    private User _user=new User();

    public LoginPageViewModel(IUserService userService, Action onLoginSuccess,Action RegisterUser)
    {
        _userService = userService;
        OnLoginSuccess = onLoginSuccess;
        Register = RegisterUser;
    }

    [RelayCommand]
    public async Task Login()
    {
        var isValid = await _userService.CheckPassword(User, User.Password);
        if (isValid)
        {
            
            _userService.CurrentUser = await _userService.GetUserByUsername(User.Login);
            _userService.Profile = await _userService.GetUserProfile(_userService.CurrentUser);
            Console.WriteLine(_userService.CurrentUser.Name);
            OnLoginSuccess?.Invoke();
        }
        else
        {
            Console.WriteLine("Неверный логин или пароль");
        }
    }

    [RelayCommand]
    private void AddUser()
    {
        Register?.Invoke();
    }
}