using System;
using System.Collections.ObjectModel;
using System.Reactive;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;
using course_work.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace course_work.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    
    private ViewModelBase _currentViewModel;
    private readonly IServiceProvider _provider;

    [ObservableProperty] private User _user;
    
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }
    
    public MainWindowViewModel(IServiceProvider provider)
    {
        _provider = provider;
        NavigateToLogin();
    }

    public void NavigateToLogin()
    {
        CurrentViewModel = ActivatorUtilities.CreateInstance<LoginPageViewModel>(_provider,OnSuccses,new Action(NavigateToRegister));
    }

    public void NavigateToRegister()
    {
        CurrentViewModel = ActivatorUtilities.CreateInstance<RegisterPageViewModel>(_provider,new Action(NavigateToLogin));
    }

    public void NavigateToMain()
    {
        CurrentViewModel = ActivatorUtilities.CreateInstance<MainPageViewModel>(
            _provider,
            _provider.GetRequiredService<IUserService>(),
            User,
            new Action(NavigateToLogin)
        );
    }

    public void OnSuccses(User user)
    {
        User = user;
        NavigateToMain();
    }

    public void LogOut()
    {
        CurrentViewModel = ActivatorUtilities.CreateInstance<LoginPageViewModel>(_provider,OnSuccses,new Action(NavigateToRegister));
    }
    
}