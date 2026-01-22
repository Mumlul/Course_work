using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace course_work.Models.Classes;

public class Module:INotifyPropertyChanged
{
    [Key]
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    private string _previewImage = "https://6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672.s3.twcstorage.ru/module.jpg";
    public string PreviewImage
    {
        get => _previewImage;
        set
        {
            if (_previewImage != value)
            {
                _previewImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PreviewImage)));
            }
        }
    }
    public Course Course { get; set; } = null!;
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}