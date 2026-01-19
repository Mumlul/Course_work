using AvRichTextBox;

namespace course_work.Models;

public class TextBlockModel : LessonBlock
{
    public string ContentJson { get; set; } = string.Empty;
    public FlowDocument? FlowDocument { get; set; }
    public TextBlockModel()
    {
        Type = LessonBlockType.Text;
    }
}