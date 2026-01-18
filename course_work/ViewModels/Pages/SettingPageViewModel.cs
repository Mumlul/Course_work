using Avalonia;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Enums;

namespace course_work.ViewModels.Pages;

public partial class SettingPageViewModel:PageViewModelBase
{
    public SettingPageViewModel()
    {
        Title = "Настройки";
        Image = "../../Assets/icons/settings.svg";
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
    
}