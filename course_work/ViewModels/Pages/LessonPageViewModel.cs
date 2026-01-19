using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using AvRichTextBox;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    [ObservableProperty] private Bitmap _image;
    [ObservableProperty] private TextBlockModel? _selectedTextBlock;
    private int _userId;

    public async override Task OnNavigatedTo()
    {
        Image = await ConvertImageToByteArray(CurrentLesson.PreviewImage);
        IsAuthor=await _lessonService.GetAuthor(CurrentLesson.Id, _userId);
        if (CurrentLesson.ContentJson != null)
        {
            var rtb = new AvRichTextBox.RichTextBox();
            rtb.LoadWordDoc(CurrentLesson.ContentJson);

            var blocks = new ObservableCollection<UIBlocks>();
            FlowDocument? currentDoc = null;

            foreach (var block in rtb.FlowDocument.Blocks)
            {
                if (block is AvRichTextBox.Paragraph par)
                {
                    var imageRun = par.Inlines.OfType<AvRichTextBox.EditableRun>()
                        .FirstOrDefault(r => Uri.IsWellFormedUriString(r.Text, UriKind.Absolute));

                    if (imageRun != null)
                    {
                        if (currentDoc != null)
                        {
                            blocks.Add(new UIBlocks
                            {
                                RText = new TextBlockModel { FlowDocument = currentDoc }
                            });
                            currentDoc = null;
                        }
                        blocks.Add(new UIBlocks
                        {
                            Image = new ImageBlockModel { ImagePath = imageRun.Text }
                        });
                    }
                    else
                    {
                        if (currentDoc == null)
                            currentDoc = new FlowDocument();

                        currentDoc.Blocks.Add(par);
                    }
                }
            }

            if (currentDoc != null)
            {
                blocks.Add(new UIBlocks
                {
                    RText = new TextBlockModel { FlowDocument = currentDoc }
                });
            }
            Blocks = blocks;
        }
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
        Console.WriteLine($"PATH:{path}");
        var url = await UploadImage(path);
        Console.WriteLine($"URL:{url}");
        
        var imageModel = new ImageBlockModel
        {
            Order = Blocks.Count,
            ImagePath = url
        };

        var block = new UIBlocks
        {
            RText = null,
            Image = imageModel
        };

        Blocks.Add(block);
    }

    /*[RelayCommand]
    public void AddImageBlock(string path)
    {
        var model = new ImageBlockModel
        {
            Order = Blocks.Count,
            ImagePath = path
        };
        Blocks.Add(new ImageBlockViewModel(model));
    }*/
    
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
        var mergedDocument = new AvRichTextBox.FlowDocument();

        foreach (var block in Blocks)
        {
            if (block.RText != null)
            {
                var doc = block.RText.FlowDocument ?? new AvRichTextBox.FlowDocument();

                foreach (var b in doc.Blocks)
                {
                    mergedDocument.Blocks.Add(b);
                }
            }
            if (block.Image != null)
            {
                var par = new AvRichTextBox.Paragraph();
                var run = new AvRichTextBox.EditableRun
                {
                    Text = block.Image.ImagePath
                };
                par.Inlines.Add(run);
                mergedDocument.Blocks.Add(par);
            }
        }
        var saveOptions = new Avalonia.Platform.Storage.FilePickerSaveOptions
        {
            Title = "Save Lesson",
            DefaultExtension = "docx",
            FileTypeChoices = new[]
            {
                new Avalonia.Platform.Storage.FilePickerFileType("Word Document") { Patterns = new[] { "*.docx" } }
            }
        };

        if (Application.Current.ApplicationLifetime
            is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
        {
            var topLevel = desktop.MainWindow;
            var file = await topLevel.StorageProvider.SaveFilePickerAsync(saveOptions);
            var path = file?.TryGetLocalPath();

            if (!string.IsNullOrEmpty(path))
            {
                var tempRtb = new AvRichTextBox.RichTextBox
                {
                    FlowDocument = mergedDocument
                };
                tempRtb.SaveWordDoc(path);
            }
        }
    }
    
    [RelayCommand]
    public async Task ChangeImage()
    {
        var file = await ChooseFile();
        if (file is null) return;
        CurrentLesson.PreviewImage =await UploadImage(file);
        Image = await ConvertImageToByteArray(CurrentLesson.PreviewImage);
        _lessonService.UpdateLesson(CurrentLesson);
    }
    
    
    
    
}