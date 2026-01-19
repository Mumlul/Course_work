using Avalonia.Media.Imaging;

namespace course_work.Models;

public class ImageBlockModel : LessonBlock
{
    public string ImagePath { get; set; } = string.Empty;
    public Bitmap Image { get; set; }

    public ImageBlockModel()
    {
        Type = LessonBlockType.Image;
    }
}