using System;
using System.Collections.ObjectModel;
using System.IO;
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
    [ObservableProperty] private bool _isReader=true;
    [ObservableProperty] private Bitmap _image;
    [ObservableProperty] private TextBlockModel? _selectedTextBlock;
    private int _userId;

    public async override Task OnNavigatedTo()
    {
        Image = await ConvertImageToByteArray(CurrentLesson.PreviewImage);
        IsAuthor=await _lessonService.GetAuthor(CurrentLesson.Id, _userId);
        IsReader = !IsAuthor;
        if (CurrentLesson.ContentUrl != null)
        {

            var path = await DownloadWordToTempAsync(CurrentLesson.ContentUrl);
            
            var rtb = new AvRichTextBox.RichTextBox();
            rtb.LoadWordDoc(path);
            Console.WriteLine(path);

            var blocks = new ObservableCollection<UIBlocks>();
            FlowDocument? currentDoc = new FlowDocument();

            foreach (var block in rtb.FlowDocument.Blocks)
            {
                if (block is AvRichTextBox.Paragraph par)
                {
                    foreach (var run in par.Inlines.OfType<AvRichTextBox.EditableRun>())
                    {
                        var lines = run.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                        foreach (var line in lines)
                        {
                            if (Uri.IsWellFormedUriString(line, UriKind.Absolute))
                            {
                                if (currentDoc.Blocks.Count > 0)
                                {
                                    blocks.Add(new UIBlocks
                                    {
                                        RText = new TextBlockModel { FlowDocument = currentDoc }
                                    });
                                    currentDoc = new FlowDocument();
                                }
                                blocks.Add(new UIBlocks
                                {
                                    Image = new ImageBlockModel { ImagePath = line }
                                });
                            }
                            else
                            {
                                var newPar = new AvRichTextBox.Paragraph();
                                newPar.Inlines.Add(new AvRichTextBox.EditableRun { Text = line });
                                currentDoc.Blocks.Add(newPar);
                            }
                        }
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
            File.Delete(path);
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
        /*var mergedDocument = new AvRichTextBox.FlowDocument();

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
        }*/
        if (_isAuthor)
        {
            var mergedDocument = new FlowDocument();
            foreach (var block in Blocks)
            {
                if (block.RText?.FlowDocument != null)
                {
                    foreach (var b in block.RText.FlowDocument.Blocks)
                    {
                        mergedDocument.Blocks.Add(b);
                    }
                }
                if (block.Image != null)
                {
                    var par = new Paragraph();
                    par.Inlines.Add(new EditableRun
                    {
                        Text = block.Image.ImagePath
                    });
                    mergedDocument.Blocks.Add(par);
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
        Image = await ConvertImageToByteArray(CurrentLesson.PreviewImage);
        _lessonService.UpdateLesson(CurrentLesson);
    }
    
    
    
    
}