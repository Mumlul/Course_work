using System;
using System.Reactive;
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
    
    [ObservableProperty] 
    private User _user=new User();

    public LoginPageViewModel(MainWindowViewModel mainWindowVm,IUserService userService)
    {
        Title = "Вход";
        _mainWindowVm = mainWindowVm;
        _userService = userService;
    }
    
    //тут надо валидацию придумать
    [RelayCommand]
    public async void Login()
    {
        if(await _userService.CheckPassword(User, User.Password))
            _mainWindowVm.GotoMain();
        else
        Console.WriteLine("не");
    }

    [RelayCommand]
    private void AddUser()
    {
        _mainWindowVm.GotoRegisterPage();
    }
}