using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;

namespace course_work.ViewModels.Pages;

public partial class TestPageViewModel:PageViewModelBase
{
    private readonly ITestService _testService;
    [ObservableProperty] private Test _currentTest;
    [ObservableProperty] private string _imageSourse=@"https://6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672.s3.twcstorage.ru/GOD.jpg";
    [ObservableProperty] private Bitmap _image;
    [ObservableProperty] private TestQuestion _selectedQuestion;

    private int _courseId;

    public override async Task OnNavigatedTo()
    {
        CurrentTest=await _testService.GetTestByCourseIdAsync(_courseId);
    }

    


    public TestPageViewModel(ITestService testService,int courseId)
    {
        Title = "Test Page";
        _testService = testService;
        _courseId=courseId;
    }

    [RelayCommand]
    public void StartTestCommand()
    {
        
    }

    [RelayCommand]
    public void GiveAnswerCommand()
    {
        
    }

    [RelayCommand]
    public async Task NextQuestion()
    {
        
    }
    
    [RelayCommand]
    public async Task LastQuestion()
    {
        
    }
    
    
    
}