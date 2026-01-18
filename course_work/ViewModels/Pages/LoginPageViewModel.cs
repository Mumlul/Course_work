using System;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models;
using course_work.Models.Classes;
using course_work.Models.Enums;
using course_work.Services;
using ReactiveUI;

namespace course_work.ViewModels.Pages;

public partial class LoginPageViewModel:PageViewModelBase
{
    private readonly MainWindowViewModel _mainWindowVm;
    private readonly IUserService _userService;
    private readonly Action<User> _onLoginSuccess;
    private readonly Action Register;
    
    [ObservableProperty]
    private string _strLogin;
    
    [ObservableProperty]
    private string password;
    
    [ObservableProperty]
    private string authError;

    public LoginPageViewModel(IUserService userService, Action<User> onLoginSuccess,Action RegisterUser)
    {
        _userService = userService;
        _onLoginSuccess = onLoginSuccess;
        Register = RegisterUser;
    }

    [RelayCommand]
    public async Task Login()
    {
        var isValid = await _userService.CheckPassword(StrLogin, Password);
        if (isValid)
        {
            
            _userService.CurrentUser = await _userService.GetUserByUsername(StrLogin);
            _userService.Profile = await _userService.GetUserProfile(_userService.CurrentUser);
            Console.WriteLine(_userService.CurrentUser.Name);
            _onLoginSuccess?.Invoke(_userService.CurrentUser);
            AuthError = null;
        }
        else
        {
            AuthError = "Неверный логин или пароль";
        }
    }

    [RelayCommand]
    private void AddUser()
    {
        AuthError = null;
        Register?.Invoke();
    }

    

    
    
}