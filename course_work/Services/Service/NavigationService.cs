using System;
using course_work.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace course_work.Services;

public class NavigationService
{
    private readonly MainWindowViewModel _mainWindowVm;
    private readonly IServiceProvider _provider;

    public NavigationService(MainWindowViewModel mainWindowVm, IServiceProvider provider)
    {
        _mainWindowVm = mainWindowVm;
        _provider = provider;
    }

    public void Navigate<T>() where T : ViewModelBase
    {
        var vm = (T)_provider.GetRequiredService(typeof(T));
        _mainWindowVm.CurrentViewModel = vm;
    }
}