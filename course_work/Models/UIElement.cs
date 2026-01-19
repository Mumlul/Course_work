using AvRichTextBox;
using CommunityToolkit.Mvvm.ComponentModel;

namespace course_work.Models;

public enum LessonBlockType
{
    Text,
    Image
}
public abstract class LessonBlock
{
    public LessonBlockType Type { get; init; }
    public int Order { get; set; }
    
}

public class UIBlocks
{
    public ImageBlockModel? Image { get; set; }
    public TextBlockModel? RText { get; set; }
}








