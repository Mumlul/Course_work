using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Models.Classes;
using course_work.Services;
using course_work.ValidationRules.Services;

namespace course_work.ViewModels.Pages;

public partial class AdminCourseClaimViewModel:PageViewModelBase
{
    private readonly ICourseService _courseService;
    private Action<Course> _openCourse;
    private Action<User> _openUser;
    
    [ObservableProperty] private static CourseComplaint _selectedComplaint;
    [ObservableProperty] private string _userMessage;
    [ObservableProperty] private string _authorMessage;
    [ObservableProperty] private bool _openWindow;
    [ObservableProperty] private int _fixDays;
    
    private CancellationTokenSource? _textCts;
    private readonly TextValidator _textValidator = new TextValidator();
    
    [ObservableProperty] private bool _canSend;


    partial void OnUserMessageChanged(string value)
    {
        _textCts?.Cancel();
        _textCts = new CancellationTokenSource();
        _ = _textValidator.Validate(
            value,
            () => ClearErrors(nameof(UserMessage)),
            error => AddError(nameof(UserMessage), error),
            _textCts.Token
        );
    }

    partial void OnAuthorMessageChanged(string value)
    {
        _textCts?.Cancel();
        _textCts = new CancellationTokenSource();
        _ = _textValidator.Validate(
            value,
            () => ClearErrors(nameof(AuthorMessage)),
            error => AddError(nameof(AuthorMessage), error),
            _textCts.Token
        );
    }
    


    public ObservableCollection<CourseComplaint> Complaints { get; set; } = new();
    
    public AdminCourseClaimViewModel(ICourseService courseService,
        Action<Course> openCourse, 
        Action<User> openUser)
    {
        Title = "Жалобы на курсы";
        ImageBlock = "../../Assets/icons/file-01.svg";
        _courseService = courseService;
        _openCourse = openCourse;
        _openUser = openUser;
        
        this.ErrorsChanged += (s, e) =>
        {
            CanSend = !HasErrors;
        };
    }

    public override async Task OnNavigatedTo()
    {
        var selectedId = SelectedComplaint?.Id;
        var claims = await _courseService.GetAllComplaints();
        
        Complaints.Clear();
        foreach (var claim in claims)
            Complaints.Add(claim);
        
        
        if (selectedId != null)
        {
            SelectedComplaint = Complaints.FirstOrDefault(c => c.Id == selectedId);
        }
    }

    partial void OnSelectedComplaintChanged(CourseComplaint value)
    {
        OpenWindow=true;
    }


    [RelayCommand]
    public async Task OpenCourse()
    {
        _openCourse?.Invoke(SelectedComplaint.Course);
    } 

    [RelayCommand]
    public async Task OpenUser() => _openUser?.Invoke(SelectedComplaint.User);

    [RelayCommand]
    public async Task SendMessageToUser()
    {
        Console.WriteLine(SelectedComplaint.User.Email);
        await SendMail(SelectedComplaint.User.Email, UserMessage);
    }

    [RelayCommand]
    public async Task SendMessageToAuthor()
    {
        var author = await _courseService.GetCourseAuthor(SelectedComplaint.CourseId);
        
        await SendMail(author.Email, UserMessage);
    }

    [RelayCommand]
    public void CloseWindow()
    {
        OpenWindow=false;
        SelectedComplaint = null;
    }
}