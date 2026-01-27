using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Models.Enums;
using course_work.Services;
using course_work.Services.Interfaces;

namespace course_work.ViewModels.Pages;

public partial class SettingPageViewModel:PageViewModelBase
{
    private readonly IUserService _userService;
    private readonly IUserProfile _userProfile;
    [ObservableProperty] private User  _user;
    [ObservableProperty] private bool _isAdmin=false;
    [ObservableProperty] private UserProfile _profile;
    
    public SettingPageViewModel(IUserService userService,User _user)
    {
        Title = "Настройки";
        ImageBlock = "../../Assets/icons/settings.svg";
        
        _userService = userService;
        User = _user;
    }

    public override async Task OnNavigatedTo()
    {
        if (User.UserTypeId != 3) _isAdmin = true;
        Profile = await _userService.GetUserProfile(User);
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
    public async Task SaveUserData()
    {
        await _userService.UpdateUser(User);
        await _userService.UpdateProfile(Profile);
    }
    
}