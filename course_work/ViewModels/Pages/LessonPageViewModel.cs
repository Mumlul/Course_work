using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using AvRichTextBox;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using course_work.Convertors;
using course_work.Models;
using course_work.Models.Classes;
using course_work.Services;

namespace course_work.ViewModels.Pages;

public partial class LessonPageViewModel:PageViewModelBase
{
    private readonly ILessonService _lessonService;
    [ObservableProperty] private Lesson _currentLesson;
    private readonly Action<Course> _backToCourse;
    public ObservableCollection<UIBlocks> Blocks { get; set; } = new();
    
    //ОБЯЗАТЕЛЬНО ПОМЕНЯТЬ СДЕЛАТЬ НОРМАЛЬНУЮ ЗАГРУЗКУ
    [ObservableProperty] private bool _isAuthor=true;
    [ObservableProperty] private bool _isReader=true;
    [ObservableProperty] private Bitmap _imageLesson;
    [ObservableProperty] private TextBlockModel? _selectedTextBlock;
    [ObservableProperty] private bool _isCompleted;
    private int _userId;

    public async override Task OnNavigatedTo()
    {
        ImageLesson = await ConvertImageToByteArray(CurrentLesson.PreviewImage);
        IsAuthor=await _lessonService.GetAuthor(CurrentLesson.Id, _userId);
        IsReader = !IsAuthor;
        if (CurrentLesson.ContentUrl != null)
        {
            var path = await DownloadWordToTempAsync(CurrentLesson.ContentUrl);
            
            var rtb = new AvRichTextBox.RichTextBox();
            rtb.LoadWordDoc(path);
            
            

            var blocks = new ObservableCollection<UIBlocks>();
            FlowDocument? currentDoc = new FlowDocument();

            
            foreach (var par in rtb.FlowDocument.Blocks.OfType<AvRichTextBox.Paragraph>())
            {
                foreach (var inline in par.Inlines)
                {
                    switch (inline)
                    {
                        case AvRichTextBox.EditableRun run:
                            var newPar = new AvRichTextBox.Paragraph();
                            newPar.Inlines.Add(new AvRichTextBox.EditableRun { Text = run.Text });
                            currentDoc.Blocks.Add(newPar);
                            break;

                        case AvRichTextBox.EditableInlineUIContainer container:
                            if (currentDoc.Blocks.Count > 0)
                            {
                                blocks.Add(new UIBlocks
                                {
                                    RText = new TextBlockModel { FlowDocument = currentDoc }
                                });
                                currentDoc = new FlowDocument();
                            }

                            if (container.Child is Image img)
                            {
                                blocks.Add(new UIBlocks
                                {
                                    Image = new ImageBlockModel
                                    {
                                        InlineUIContainer = container,
                                        Image = img.Source as Bitmap,
                                        ImagePath = "" 
                                    }
                                });
                            }
                            break;
                    }
                }
            }

            if (currentDoc.Blocks.Count > 0)
            {
                blocks.Add(new UIBlocks
                {
                    RText = new TextBlockModel { FlowDocument = currentDoc }
                });
            }

            Blocks.Clear();
            foreach (var b in blocks)
                Blocks.Add(b);
        }
        
        IsCompleted=await _lessonService.IsCompleteLesson(CurrentLesson.Id, _userId);
    }
    
    public LessonPageViewModel(ILessonService lessonService,Lesson lesson,Action<Course> backtocourse,int userid)
    {
        Title = "Lesson";
        _lessonService = lessonService;
        CurrentLesson = lesson;
        _backToCourse = backtocourse;
        _userId = userid;
    }

    [RelayCommand]
    public async Task CloseLesson()
    {
        var course = await _lessonService.GetCourse(CurrentLesson.Id);
        _backToCourse?.Invoke(course);
    }
    [RelayCommand]
    public void AddTextBlock()
    {
        var textModel = new TextBlockModel
        {
            Order = Blocks.Count
        };

        var block = new UIBlocks
        {
            RText = textModel,
            Image = null
        };

        Blocks.Add(block);
    }
    
    [RelayCommand]
    public async Task AddImageBlock()
    {
        var path = await ChooseFile();
        if (string.IsNullOrEmpty(path)) return;

        var url = await UploadImage(path);
        var bitmap = new Bitmap(path);
        var imgControl = new Avalonia.Controls.Image
        {
            Source = bitmap,
            Stretch = Avalonia.Media.Stretch.Uniform
        };

        var inlineContainer = new EditableInlineUIContainer(imgControl);
        
        var imageModel = new ImageBlockModel
        {
            Order = Blocks.Count,
            ImagePath = url,
            Image = bitmap,
            InlineUIContainer = inlineContainer
        };

        Blocks.Add(new UIBlocks
        {
            RText = null,
            Image = imageModel
        });
    }
    
    public void ApplyFontColor(Color color)
    {
        if (SelectedTextBlock?.FlowDocument == null)
            return;
        SelectedTextBlock.FlowDocument.Selection.ApplyFormatting(
            AvRichTextBox.RichTextBox.ForegroundProperty,
            new Avalonia.Media.SolidColorBrush(color));
    }

    public void ApplyHighlightColor(Color color)
    {
        if (SelectedTextBlock?.FlowDocument == null)
            return;
        SelectedTextBlock.FlowDocument.Selection.ApplyFormatting(
            AvRichTextBox.RichTextBox.BackgroundProperty,
            new Avalonia.Media.SolidColorBrush(color));
    }

    public void ApplyFontSize(double size)
    {
        if (SelectedTextBlock?.FlowDocument == null)
            return;
        SelectedTextBlock.FlowDocument.Selection.ApplyFormatting(
            AvRichTextBox.RichTextBox.FontSizeProperty,
            size);
    }

    public void ApplyJustification(string justification)
    {
        if (SelectedTextBlock?.FlowDocument == null)
            return;
        var par = SelectedTextBlock.FlowDocument.Selection.GetStartPar();
        if (par != null)
        {
            par.TextAlignment = justification switch
            {
                "Left" => Avalonia.Media.TextAlignment.Left,
                "Center" => Avalonia.Media.TextAlignment.Center,
                "Right" => Avalonia.Media.TextAlignment.Right,
                "Justified" => Avalonia.Media.TextAlignment.Justify,
                _ => Avalonia.Media.TextAlignment.Left
            };
        }
    }
    
    
    
    [RelayCommand]
    public async Task SaveAllBlocksToWordAsync()
    {
        if (_isAuthor)
        {
            var mergedDocument = new FlowDocument();

            foreach (var block in Blocks)
            {
                if (block.RText?.FlowDocument != null)
                {
                    foreach (var textBlock in block.RText.FlowDocument.Blocks)
                    {
                        mergedDocument.Blocks.Add(textBlock);
                    }
                }

                if (block.Image != null)
                {
                    var paragraph = new Paragraph();

                    Image imgControl;

                    if (block.Image.InlineUIContainer != null)
                    {
                        paragraph.Inlines.Add(block.Image.InlineUIContainer);
                    }
                    else
                    {
                        imgControl = new Image
                        {
                            Stretch = Avalonia.Media.Stretch.None,
                            Width = 100,
                            Height = 5
                        };
                        
                        imgControl.Bind(Image.SourceProperty, new Avalonia.Data.Binding("ImagePath")
                        {
                            Source = block.Image,
                            Converter = new UrlToBitmap()
                        });

                        var inlineContainer = new EditableInlineUIContainer(imgControl);
                        block.Image.InlineUIContainer = inlineContainer; // кешируем

                        paragraph.Inlines.Add(inlineContainer);
                    }

                    mergedDocument.Blocks.Add(paragraph);
                }
            }

            var fileName = await _lessonService.GetLessonFileName(CurrentLesson.Id);
            var tempPath = Path.Combine(Path.GetTempPath(), fileName);

            try
            {
                var tempRtb = new RichTextBox
                {
                    FlowDocument = mergedDocument
                };

                tempRtb.SaveWordDoc(tempPath);
                var url = await UploadWordToTempAsync(tempPath);
                CurrentLesson.ContentUrl = url;
                await _lessonService.UpdateLesson(CurrentLesson);
            }
            finally
            {
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
        }
        
        var course = await _lessonService.GetCourse(CurrentLesson.Id);
        _backToCourse?.Invoke(course);
    }
    
    [RelayCommand]
    public async Task ChangeImage()
    {
        var file = await ChooseFile();
        if (file is null) return;
        CurrentLesson.PreviewImage =await UploadImage(file);
        ImageLesson = await ConvertImageToByteArray(CurrentLesson.PreviewImage);
        _lessonService.UpdateLesson(CurrentLesson);
    }
    
    [RelayCommand]
    public async Task CompleteLesson()
    {
        Console.WriteLine(IsCompleted);
        await _lessonService.CompleteLesson(lessonId: CurrentLesson.Id, userId: _userId,completed: IsCompleted);
        await _lessonService.UpdateCourseProgress(await _lessonService.GetCurseId(CurrentLesson.Id), _userId);
    }
    
    
    
    
}