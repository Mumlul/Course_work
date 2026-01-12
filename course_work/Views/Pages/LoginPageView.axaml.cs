using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.ViewModels;
using course_work.ViewModels.Pages;

namespace course_work.Views.Pages;

public partial class LoginPageView : UserControl
{
    public LoginPageView()
    {
        InitializeComponent();
    }

    [RelayCommand]
    private void GoToMainPage()
    {
        
    }
}