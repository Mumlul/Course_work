
using CommunityToolkit.Mvvm.ComponentModel;
using course_work.Models;

namespace course_work.ViewModels;

public partial class TextBlockViewModel : ObservableObject
{
    public TextBlockModel Model { get; }

    public AvRichTextBox.RichTextBox? RichTextBox { get; set; }

    public TextBlockViewModel(TextBlockModel model)
    {
        Model = model;
    }

    public AvRichTextBox.FlowDocument? GetFlowDocument()
    {
        return RichTextBox?.FlowDocument;
    }
}