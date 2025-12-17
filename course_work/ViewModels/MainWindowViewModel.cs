using System;
using System.Collections.ObjectModel;
using System.Reactive;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }
    
    public MainWindowViewModel(IServiceProvider provider)
    {
        _provider = provider;
        CurrentViewModel = ActivatorUtilities.CreateInstance<LoginPageViewModel>(_provider, this);
    }

    [RelayCommand]
    private void MainPageCommand()
    {
        
    }

    
    public void GotoRegisterPage()
    {
        Console.WriteLine("RegisterPageCommand called");
        CurrentViewModel = _provider.GetRequiredService<RegisterPageViewModel>();
        Console.WriteLine("CurrentViewModel set to " + CurrentViewModel.GetType().Name);
    }

    [RelayCommand]
    private void TestCommand()
    {
        
    }
    
    public void GotoMain()
    {
        Console.WriteLine("GotoMain called");
        CurrentViewModel = new MainPageViewModel();
        Console.WriteLine("CurrentViewModel set to " + CurrentViewModel.GetType().Name);
    }
}