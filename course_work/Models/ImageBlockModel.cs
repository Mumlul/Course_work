using Avalonia.Media.Imaging;
using AvRichTextBox;

namespace course_work.Models;

public class ImageBlockModel : LessonBlock
{
    public string ImagePath { get; set; } = string.Empty;
    public Bitmap Image { get; set; }
    public EditableInlineUIContainer  InlineUIContainer { get; set; }

    public ImageBlockModel()
    {
        Type = LessonBlockType.Image;
    }
}