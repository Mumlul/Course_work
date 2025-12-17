using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels.Pages;

public partial class RegisterPageViewModel:PageViewModelBase
{
    private readonly IUserService _userService;
    
    [ObservableProperty]
    private User _user = new User();
    
    public RegisterPageViewModel(IUserService userService)
    {
        _userService=userService;
        Title = "Register Page";
    }


    [RelayCommand]
    private void Register()
    {
        
        //Сделать как то по хорошему как будет роль выбиратсья просто пока не придумал
        var _newUser = new User()
        {
            Name = User.Name,
            Login = User.Login,
            Email = User.Email,
            Password = User.Password,
            CreatedAt = DateTime.Today,
            UserTypeId = 1,
        };

        _userService.AddUser(_newUser);
    }
}