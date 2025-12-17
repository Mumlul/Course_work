using CommunityToolkit.Mvvm.ComponentModel;

namespace course_work.ViewModels.Pages;

public partial class PageViewModelBase:ViewModelBase
{
    public string? Title { get; set; }
    public string? Image { get; set; }
    
    [ObservableProperty]
    public bool _textVisible  = true;
}