using System;
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
    private readonly int _courseId;

    private readonly Action<Course> _navigateback;
    
    [ObservableProperty] private Test _test;
    [ObservableProperty] private Bitmap _image;
    [ObservableProperty] private Course _course;
    [ObservableProperty] private int _countQuestions;
    [ObservableProperty] private TestQuestion _selectedQuestion;
    [ObservableProperty] private TestQuestionOption _selectedOption;
    [ObservableProperty] private string _questionText;
    [ObservableProperty] private int _pasScore;
    public ObservableCollection<TestQuestionOption> QuestionOptions { get; } = new();
    public ObservableCollection<TestQuestion> Questions { get; } = new();


    


    public async override Task OnNavigatedTo()
    {
        Test = await _testService.GetTestByCourseIdAsync(_courseId);

        foreach (var question in Test.Questions)
            Questions.Add(question);
        
    }

    public async override Task OnNavigatedFrom()
    {
        await _testService.UpdateTestAsync(Test);
    }

    

    public CreateTestPageViewModel(ITestService testService,int course,Action<Course> navigateback)
    {
        Title = "Create Test";
        _testService = testService;
        _courseId=course;
        _navigateback=navigateback;
    }

    [RelayCommand]
    public async Task CreateTestAsync()
    {
        await _testService.CreateTestAsync(_test);
    }

    [RelayCommand]
    public async Task CreateQuestionAsync()
    {
        var question = new TestQuestion()
        {
            TestId = Test.Id,
            QuestionText = "",
            QuestionType = QuestionType.SingleChoice,
            OrderIndex = Test.Questions.Count+1,
        };
        
        _testService.AddQuestionAsync(Test.Id,question);
        Questions.Add(question);
    }

    [RelayCommand]
    public async Task UploadImageAsync()
    {
        var file = await ChooseFile();
        Image = await ConvertImageToByteArray(file);
    }


    [RelayCommand]
    public async Task AddOptionAsync()
    {
        var op = new TestQuestionOption()
        {
            QuestionId = SelectedQuestion.Id,
            OptionText = "",
            IsCorrect = false,
            OrderIndex = QuestionOptions.Count+1
        };

        await _testService.AddOptionAsync(op);
        QuestionOptions.Add(op);
    }
    
    [RelayCommand]
    public async Task DeleteOptionAsync()
    {
        
    }

    [RelayCommand]
    public async Task UpdateQuestionAsync()
    {
        await _testService.UpdateQuestionAsync(SelectedQuestion);
        foreach (var op in QuestionOptions)
            await _testService.UpdateOptionAsync(op);
    }
    
    [RelayCommand]
    public async Task DeleteQuestionAsync()
    {
        foreach (var op in QuestionOptions)
            await _testService.DeleteOptionAsync(op.Id);
        QuestionOptions.Clear();
        await _testService.DeleteQuestionAsync(SelectedQuestion.Id);
        Questions.Remove(SelectedQuestion);
        
        for (int i = 0; i < Questions.Count; i++)
        {
            var q = Questions[i];
            q.OrderIndex = i + 1; 
            await _testService.UpdateQuestionAsync(q);
        }
        SelectedQuestion=new ();
    }
    
    [RelayCommand]
    public void SelectCorrectOption(TestQuestionOption selectedOption)
    {
        if (SelectedQuestion == null)
            return;

        foreach (var option in QuestionOptions)
            option.IsCorrect = false;

        selectedOption.IsCorrect = true;
    }

    partial  void OnSelectedQuestionChanged(TestQuestion value)
    {
        LoadingOptions(value.Id);
    }
    
    private async void LoadingOptions(int id)
    {
        QuestionOptions.Clear();
        var opt=await _testService.GetQuestionOptionsAsync(id);
        foreach (var option in opt)
            QuestionOptions.Add(option);
    }
    
}