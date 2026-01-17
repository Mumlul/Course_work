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

    public ObservableCollection<TestQuestion> TestQuestions { get; } = new();
    
    public ObservableCollection<TestQuestionOption> Options { get; set; }
    [ObservableProperty] private string _imageSourse=@"https://6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672.s3.twcstorage.ru/GOD.jpg";
    [ObservableProperty] private Bitmap _image;
    [ObservableProperty] private TestQuestion _currentQuestion;

    public override async Task OnNavigatedTo()
    {
        //тут сделать загрузку вопросов в TestQuestions

        // В тест добавить изображение надо поле именно!!!!
        Image = await ConvertImageToByteArray(ImageSourse);
    }

    partial void OnCurrentQuestionChanged(TestQuestion value)
    {
        //тут прописывать что бы загружались варианты ответа для вопроса
    }


    public TestPageViewModel(ITestService testService,Test test)
    {
        Title = "Test Page";
        _testService = testService;
        CurrentTest = test;
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
    public async Task NextQuestionCommand()
    {
        
    }
    
    [RelayCommand]
    public async Task LastQuestionCommand()
    {
        
    }
    
    
    
}