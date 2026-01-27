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

public partial class AdminUserClaimViewModel:PageViewModelBase
{
    
    private readonly IUserService _userService;
    private readonly Action<User> _userAction;
    
    [ObservableProperty] private static UserComplaint _selectedComplaint;
    [ObservableProperty] private bool _openWindow;
    [ObservableProperty] private string _userMessage;
    [ObservableProperty] private string _criminalMessage;
    [ObservableProperty] private int _fixDays;

    
    private CancellationTokenSource? _textCts;
    private readonly TextValidator _textValidator = new TextValidator();
    
    [ObservableProperty] private bool _canSend;
    
    public ObservableCollection<UserComplaint> Complaints {get; set; }=new ();


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

    partial void OnCriminalMessageChanged(string value)
    {
        _textCts?.Cancel();
        _textCts = new CancellationTokenSource();
        _ = _textValidator.Validate(
            value,
            () => ClearErrors(nameof(CriminalMessage)),
            error => AddError(nameof(CriminalMessage), error),
            _textCts.Token
        );
    }


    partial void OnSelectedComplaintChanged(UserComplaint value)
    {
        OpenWindow = true;
    }


    public AdminUserClaimViewModel( IUserService userService,Action<User> openUser)
    {
        Title = "Жалобы на пользователей";
        ImageBlock = "../../Assets/icons/user-profile-03.svg";
        _userService = userService;
        _userAction = openUser;
        
        this.ErrorsChanged += (s, e) =>
        {
            CanSend = !HasErrors;
        };
    }

    [RelayCommand]
    public async Task OpenUserFrom()
    {
        _userAction?.Invoke(SelectedComplaint.FromUser);
    }

    [RelayCommand]
    public async Task OpenUserTo()
    {
        _userAction?.Invoke(SelectedComplaint.ToUser);
    }

    [RelayCommand]
    public async Task CloseWindow()
    {
        OpenWindow=false;
        SelectedComplaint=null;
    }

    [RelayCommand]
    public async Task SendMessageToUser()
    {
        await SendMail(SelectedComplaint.FromUser.Email, UserMessage);
    }

    [RelayCommand]
    public async Task SendMessageToCriminal()
    {
        await SendMail(SelectedComplaint.ToUser.Email, CriminalMessage);
    }

    public async override Task OnNavigatedTo()
    {
        var selectedId = SelectedComplaint?.Id;
        
        var complains=await _userService.GetAllComplaints();
        Complaints.Clear();
        foreach (var complain in complains)
            Complaints.Add(complain);
        Console.WriteLine(Complaints.Count);
        if(selectedId!=null) SelectedComplaint=Complaints.FirstOrDefault(c=>c.Id == selectedId);
    }
}