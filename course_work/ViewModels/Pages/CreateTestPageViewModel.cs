using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;


namespace course_work.ViewModels.Pages;

public partial class CreateTestPageViewModel:PageViewModelBase
{
    private readonly ITestService _testService;

    [ObservableProperty] private Test _test;
    [ObservableProperty] private Bitmap _image;
    [ObservableProperty] private Course _course;
    [ObservableProperty] private int _countQuestions;
    [ObservableProperty] private TestQuestion _selectedQuestion;
    [ObservableProperty] private TestQuestionOption _selectedOption;
    [ObservableProperty] private string _questionText;
    
    public ObservableCollection<TestQuestionOption> Options { get; } = new();
    public ObservableCollection<TestQuestion> Questions { get; } = new();
    
    
    public override Task OnNavigatedTo()
    {
        return base.OnNavigatedTo();
    }

    public CreateTestPageViewModel(ITestService testService)
    {
        Title = "Create Test";
        _testService = testService;
    }

    [RelayCommand]
    public async Task CreateTestAsync()
    {
        await _testService.CreateTestAsync(_test);
    }

    [RelayCommand]
    public async Task CreateQuestionAsync()
    {
        
    }

    [RelayCommand]
    public async Task UploadImageAsync()
    {
        var file = await ChooseFile();
        Image = await ConvertImageToByteArray(file);
    }

    [RelayCommand]
    public async Task GoNextQuestionAsync()
    {
        
    }
    [RelayCommand]
    public async Task GoBackQuestionAsync()
    {
        
    }

    [RelayCommand]
    public async Task AddOptionAsync()
    {
        
    }
    
    [RelayCommand]
    public async Task DeleteOptionAsync()
    {
        
    }

    [RelayCommand]
    public async Task AddQuestionAsync()
    {
        
    }

    [RelayCommand]
    public async Task DeleteQuestionAsync()
    {
        
    }
}