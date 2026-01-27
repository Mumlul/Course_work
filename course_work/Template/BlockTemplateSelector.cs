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
                    // Если в модели уже есть InlineUIContainer, возвращаем его в RichTextBox
                    if (block.Image.InlineUIContainer != null)
                    {
                        var rtb = new RichTextBox
                        {
                            FlowDocument = new FlowDocument()
                        };

                        var par = new Paragraph();
                        par.Inlines.Add(block.Image.InlineUIContainer);
                        rtb.FlowDocument.Blocks.Add(par);

                        rtb.IsReadOnly = true;
                        return rtb;
                    }

                    // Иначе создаем новый Image и оборачиваем его в InlineUIContainer
                    var imgControl = new Image
                    {
                        [!Image.SourceProperty] = new Binding("ImagePath") { Converter = new UrlToBitmap() },
                        Stretch = Stretch.None,
                        Width = 100, // Можно задать нужный размер
                        Height = 50
                    };

                    var inlineContainer = new EditableInlineUIContainer(imgControl);
                    block.Image.InlineUIContainer = inlineContainer;

                    var rtbWithImage = new RichTextBox
                    {
                        FlowDocument = new FlowDocument(),
                        IsReadOnly = true
                    };

                    var paragraph = new Paragraph();
                    paragraph.Inlines.Add(inlineContainer);
                    rtbWithImage.FlowDocument.Blocks.Add(paragraph);

                    return rtbWithImage;
                }
            }

            return new TextBlock { Text = "Unknown block" };
        }

        public bool Match(object? data) => true;
    }
}