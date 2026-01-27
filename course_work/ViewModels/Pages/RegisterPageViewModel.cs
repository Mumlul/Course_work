using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;
using course_work.ValidationRules.Interfaces;
using course_work.ValidationRules.Services;
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
    
    [ObservableProperty] private string _login;
    [ObservableProperty] private string _email;
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _password;
    [ObservableProperty] private bool _canReg;
    
    private CancellationTokenSource? _loginCts;
    private CancellationTokenSource? _emailCts;
    private CancellationTokenSource? _pasCts;
    private CancellationTokenSource? _textCts;
    private readonly EmailValidator _emailValidator = new EmailValidator();
    private readonly LoginValidator _loginValidator = new LoginValidator();
    private readonly PasswordValidator _passwordValidator = new PasswordValidator();
    private readonly TextValidator _textValidator = new TextValidator();

        
    
    
    private Action _goToLoginPage;

    partial void OnIsAuthotChanged(bool value)
    {
        Console.WriteLine(value);
    }
    
    

    partial void OnLoginChanged(string value)
    {
        _loginCts?.Cancel();
        _loginCts = new CancellationTokenSource();

        _ = _loginValidator.ValidateAsync(
            value,
            _userService.CheckLogin,
            () => ClearErrors(nameof(Login)),
            error=>AddError(nameof(Login), error),
            _loginCts.Token
        );
        
    }

    partial void OnEmailChanged(string value)
    {
        _emailCts?.Cancel();
        _emailCts = new CancellationTokenSource();

        _ = _emailValidator.ValidateAsync(
            value,
            _userService.CheckEmail,
            () => ClearErrors(nameof(Email)),
            error => AddError(nameof(Email), error),
            _emailCts.Token);
    }

    partial void OnPasswordChanged(string value)
    {
        _pasCts?.Cancel();
        _pasCts = new CancellationTokenSource();

        _ = _passwordValidator.Validate(
            value,
            () => ClearErrors(nameof(Password)),
            error => AddError(nameof(Password), error),
            _pasCts.Token);
    }

    partial void OnConfirmPasswordChanged(string value)
    {
        _passwordValidator.ValidateConfirmPassword(
            Password,   
            value,
            () => ClearErrors(nameof(ConfirmPassword)),
            error => AddError(nameof(ConfirmPassword), error)
        );
    }

    partial void OnSecretCodeChanged(string value)
    {
        _passwordValidator.ValidateConfirmCode(
            _secretcodeSend,        
            value,           
            () => ClearErrors(nameof(SecretCode)), 
            error => AddError(nameof(SecretCode), error)
        );
    }

    partial void OnNameChanged(string value)
    {
        _textCts?.Cancel();
        _textCts = new CancellationTokenSource();
        _ = _textValidator.Validate(
        value,
        () => ClearErrors(nameof(Name)),
        error => AddError(nameof(Name), error),
        _textCts.Token
            );
    }

    public RegisterPageViewModel(IUserService userService,Action goToLoginPage)
    {
        _userService=userService;
        
        Title = "Register Page";
        _goToLoginPage = goToLoginPage;
        
        this.ErrorsChanged += (s, e) =>
        {
            CanReg = !HasErrors;
        };
    }
    
    [RelayCommand] private void CloseDialog()=> IsOpen = false;
    [RelayCommand] private async Task Register()
    {
        _secretcodeSend= await SendSecretCode(Email);
        IsOpen = true;
    }
    [RelayCommand] private void GoToLoginPage()
    {
       _goToLoginPage?.Invoke();
    }
    
    [RelayCommand] private void CheckPassword()
    {
        if (string.IsNullOrWhiteSpace(_secretcodeSend) || _secretcodeSend != SecretCode) { return;}
        
        var _newUser = new User()
        {
            Name = Name,
            Login = Login,
            Email = Email,
            Password = Password,
            CreatedAt = DateTime.Today,
            UserTypeId = IsAuthot switch
            {
                true => 2,
                false=>1
            }
        };

        _userService.AddUser(_newUser,_newUser.Password);
    }
    
}