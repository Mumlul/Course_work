using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using course_work.Models;
using AvRichTextBox;
using course_work.Convertors;
using course_work.ViewModels.Pages;

namespace course_work.Template
{
    public class BlockTemplateSelector : IDataTemplate
    {
        private readonly LessonPageViewModel _pageVm;

        public BlockTemplateSelector(LessonPageViewModel pageVm)
        {
            _pageVm = pageVm;
        }
        
        
        public Control? Build(object? data)
        {
            if (data is UIBlocks block)
            {
                if (block.RText != null)
                {
                    var rtb = new RichTextBox
                    {
                        DataContext = block.RText
                    };

                    rtb.Loaded += (s, e) =>
                    {
                        if (block.RText.FlowDocument != null)
                            rtb.FlowDocument = block.RText.FlowDocument;
                    };
                    
                    if(_pageVm.IsAuthor) rtb.IsReadOnly = false;
                    else rtb.IsReadOnly = true;
                    
                    rtb.GotFocus += (s, e) =>
                    {
                        _pageVm.SelectedTextBlock = block.RText;
                    };

                    return rtb;
                }

                if (block.Image != null)
                {
                    var image = new Image
                    {
                        DataContext = block.Image,
                        [!Image.SourceProperty] = new Binding("ImagePath") { Converter = new UrlToBitmap() },
                        Stretch = Stretch.UniformToFill
                    };
                    return image;
                }
            }

            return new TextBlock { Text = "Unknown block" };
        }

        public bool Match(object? data) => true;
    }
}