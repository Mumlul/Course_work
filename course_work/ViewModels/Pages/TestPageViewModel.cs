using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    [ObservableProperty] private User _CurrentUser;

    private int _courseId;

    public override async Task OnNavigatedTo()
    {
        CurrentTest=await _testService.GetTestByCourseIdAsync(_courseId);
    }

    public override Task OnNavigatedFrom()
    {
        Console.WriteLine($"ZAKRIL");
        return Task.CompletedTask;
    }

    public TestPageViewModel(ITestService testService,int courseId,User currentUser)
    {
        Title = "Test Page";
        _testService = testService;
        _courseId=courseId;
        CurrentUser=currentUser;
    }
    
    [RelayCommand]
    public async Task FinishTest()
    {
        var (score,pas) = CheckTest();

        if (pas)
        {
            Console.WriteLine("прошел");
        }
        else
        {
            Console.WriteLine("yt ghjitk");
        }
        
        
        
    }


    private (double,bool) CheckTest()
    {
        int totalPoints = 0;
        int scoredPoints = 0;

        foreach (var question in CurrentTest.Questions)
        {
            totalPoints += question.Points;

            bool allCorrectSelected = question.Options
                .Where(o => o.IsCorrect)
                .All(o => o.IsSelected);

            bool noIncorrectSelected = question.Options
                .Where(o => !o.IsCorrect)
                .All(o => !o.IsSelected);

            if (allCorrectSelected && noIncorrectSelected)
            {
                scoredPoints += question.Points;
            }
        }

        double scorePercent = ((double)scoredPoints / totalPoints) * 100;
        bool passed = scorePercent >= CurrentTest.PassingScore;

        var resultMessage = passed
            ? $"Тест сдан! Результат: {scorePercent:F2}%"
            : $"Тест не сдан. Результат: {scorePercent:F2}%";
        
        Console.WriteLine(resultMessage);
        
        return (scorePercent,passed ? true : false);
    }
    
}