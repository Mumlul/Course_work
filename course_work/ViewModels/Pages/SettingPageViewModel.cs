using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Models.Enums;
using course_work.Services;

namespace course_work.ViewModels.Pages;

public partial class SettingPageViewModel:PageViewModelBase
{
    private readonly IUserService _userService;
    [ObservableProperty] private User  _user;
    
    public SettingPageViewModel(IUserService userService)
    {
        Title = "Настройки";
        Image = "../../Assets/icons/settings.svg";
        
        _userService = userService;
        User = _userService.CurrentUser;
    }

    [RelayCommand]
    public void ChL()
    {
        ((App)Application.Current!).SetTheme(AppTheme.Light);
    }
    [RelayCommand]
    public void ChDb()
    {
        ((App)Application.Current!).SetTheme(AppTheme.DarkBlue);
    }
    [RelayCommand]
    public void ChDg()
    {
        ((App)Application.Current!).SetTheme(AppTheme.DarkGraphite);
    }

    [RelayCommand]
    public void SaveUserData()
    {
        _userService.UpdateUser(User);
    }
    
}