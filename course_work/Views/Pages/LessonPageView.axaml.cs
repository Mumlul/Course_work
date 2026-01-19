using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using AvRichTextBox;
using course_work.Models;
using course_work.Template;
using course_work.ViewModels;
using course_work.ViewModels.Pages;


namespace course_work.Views.Pages;

public partial class LessonPageView : UserControl
{
    public LessonPageView()
    {
        InitializeComponent();
        this.Loaded+=OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is LessonPageViewModel vm)
        {
            BlocksItemsControl.ItemTemplate = new BlockTemplateSelector(vm);
        }
    }


    private void FontCP_ColorChanged(object? sender, ColorChangedEventArgs e)
    {
        if (DataContext is LessonPageViewModel vm)
        {
            vm.ApplyFontColor(e.NewColor);
        }
    }

    private void HighlightCP_ColorChanged(object? sender, ColorChangedEventArgs e)
    {
        if (DataContext is LessonPageViewModel vm)
        {
            vm.ApplyHighlightColor(e.NewColor);
        }
    }

    private void FontSizeNS_UserValueChanged(double value)
    {
        if (DataContext is LessonPageViewModel vm)
        {
            vm.ApplyFontSize(value);
        }
    }

    private void JustificationComboBox_DropDownClosed(object? sender, EventArgs e)
    {
        if (sender is Avalonia.Controls.ComboBox cbox && cbox.SelectedItem is Avalonia.Controls.ComboBoxItem cbitem)
        {
            if (cbitem.Content is string selJust && DataContext is LessonPageViewModel vm)
            {
                vm.ApplyJustification(selJust);
            }
        }
    }
    
    private void RichTextBox_Loaded(object? sender, RoutedEventArgs e)
    {
        if (sender is AvRichTextBox.RichTextBox rtb &&
            rtb.DataContext is TextBlockModel model)
        {
            if (model.FlowDocument == null)
                model.FlowDocument = new AvRichTextBox.FlowDocument();

            rtb.FlowDocument = model.FlowDocument;
        }
    }

    private void RichTextBox_GotFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is AvRichTextBox.RichTextBox rtb &&
            rtb.DataContext is UIBlocks block &&
            block.RText != null &&
            DataContext is LessonPageViewModel pageVm)
        {
            pageVm.SelectedTextBlock = block.RText;
            Console.WriteLine("SelectedTextBlock: " + pageVm.SelectedTextBlock);
        }
    }
}