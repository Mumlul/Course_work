using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;


namespace course_work.ViewModels.Pages;

public partial class AdminUserClaimViewModel:PageViewModelBase
{
    
    private readonly IUserService _userService;
    private readonly Action<User> _userAction;
    
    [ObservableProperty] private UserComplaint _selectedComplaint;
    [ObservableProperty] private bool _openWindow;
    [ObservableProperty] private string _userMessage;
    [ObservableProperty] private string _criminalMessage;
    [ObservableProperty] private int _fixDays;


    ObservableCollection<UserComplaint> Complaints {get; set; }=new ();

    public AdminUserClaimViewModel( IUserService userService,Action<User> openUser)
    {
        Title = "Жалобы на пользователей";
        Image = "../../Assets/icons/user-profile-03.svg";
        _userService = userService;
        _userAction = openUser;
    }

    [RelayCommand]
    public async Task OpenUserFrom()
    {
        _userAction?.Invoke(SelectedComplaint.FromUser);
    }

    [RelayCommand]
    public async Task OpenUserTo()
    {
        _userAction?.Invoke(SelectedComplaint.ToUser);
    }

    [RelayCommand]
    public async Task CloseWindow()
    {
        OpenWindow=false;
        SelectedComplaint=null;
    }

    [RelayCommand]
    public async Task SendMessageToUser()
    {
        
    }

    [RelayCommand]
    public async Task SendMessageToCriminal()
    {
        
    }
    
}