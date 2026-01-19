using CommunityToolkit.Mvvm.ComponentModel;
using course_work.Models;

namespace course_work.ViewModels;

public partial class ImageBlockViewModel : ObservableObject
{
    public ImageBlockModel Model { get; }

    [ObservableProperty]
    private string imagePath;

    public ImageBlockViewModel(ImageBlockModel model)
    {
        Model = model;
        imagePath = model.ImagePath;
    }

    partial void OnImagePathChanged(string value)
    {
        ((ImageBlockModel)Model).ImagePath = value;
    }
}

