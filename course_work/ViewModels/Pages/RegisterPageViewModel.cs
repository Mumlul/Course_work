using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels.Pages;

public partial class RegisterPageViewModel:PageViewModelBase
{
    private readonly IUserService _userService;
    [ObservableProperty] private bool _isOpen = false;
    [ObservableProperty] private string _secretCode;
    [ObservableProperty] private string _confirmPassword;
    [ObservableProperty] private User _user = new User();
    private string _secretcodeSend;
    [ObservableProperty] private bool _isAuthot = false;
    
    
    private Action _goToLoginPage;

    partial void OnIsAuthotChanged(bool value)
    {
        Console.WriteLine(value);
    }

    public RegisterPageViewModel(IUserService userService,Action goToLoginPage)
    {
        _userService=userService;
        
        Title = "Register Page";
        _goToLoginPage = goToLoginPage;
    }
    
    [RelayCommand] private void CloseDialog()=> IsOpen = false;
    [RelayCommand] private async Task Register()
    {
        _secretcodeSend= await SendSecretCode(User.Email);
        IsOpen = true;
    }
    [RelayCommand] private void GoToLoginPage()
    {
       _goToLoginPage?.Invoke();
    }
    
    [RelayCommand] private void CheckPassword()
    {
        if (string.IsNullOrWhiteSpace(_secretcodeSend) || _secretcodeSend != SecretCode) { return;}
        
        //Сделать как то по хорошему как будет роль выбиратсья просто пока не придумал
        var _newUser = new User()
        {
            Name = User.Name,
            Login = User.Login,
            Email = User.Email,
            Password = User.Password,
            CreatedAt = DateTime.Today,
            UserTypeId = IsAuthot switch
            {
                true => 2,
                false=>1
            }
        };

        _userService.AddUser(_newUser);
        /*_mainWindowVm.GotoLoginPage();*/
    }
}