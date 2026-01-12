using System;
using CommunityToolkit.Mvvm.ComponentModel;
using course_work.Models.Classes;
using course_work.Services;

namespace course_work.ViewModels.Pages;

public partial class LessonPageViewModel:PageViewModelBase
{
    private readonly ILessonService _lessonService;
    [ObservableProperty] private Lesson _currentLesson;
    
    
    public LessonPageViewModel(ILessonService lessonService,Lesson lesson)
    {
        Title = "Lesson";
        _lessonService = lessonService;
        CurrentLesson = lesson;
    }
}